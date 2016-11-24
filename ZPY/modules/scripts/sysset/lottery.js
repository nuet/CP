define(function (require, exports, module) {
    var Global = require("global"),
        doT = require("dot"),
        Verify = require("verify"), VerifyObject,
        Easydialog = require("easydialog");
    require("switch");
    var Model = {},
        cacheMenu = [];

    var ObjectJS = {};
    //初始化
    ObjectJS.init = function () {
        var _self = this;
        _self.bindEvent();
        _self.getList();
    }
    //绑定事件
    ObjectJS.bindEvent = function () {
        var _self = this;

        $(document).click(function (e) {
            //隐藏下拉
            if (!$(e.target).parents().hasClass("dropdown-ul") && !$(e.target).parents().hasClass("dropdown") && !$(e.target).hasClass("dropdown")) {
                $(".dropdown-ul").hide();
            }
        });

        $("#createModel").click(function () {
            var _this = $(this);
            Model.AutoID = -1;
            Model.CPCode = '';
            Model.CPName = '';
            Model.IconType ='';
            //Model.Sort = 0;
            Model.ResultUrl ='';
            _self.createModel();
        });
        //删除
        //$("#deleteObject").click(function () {
        //    var id = $(this).data("id");
        //    confirm("暂停销售后用户表不可购买此彩种,确认暂停销售吗？", function () {
        //        _self.deleteModel(id, function(status) {
        //            if (status == 1) {
        //                _self.getList();
        //            } else {
        //                alert("执行失败请联系管理员");
        //            }
        //        });
        //    });
        //}); 
        //编辑
        $("#updateObject").click(function () {
            var _this = $(this);
            Global.post("/SysSet/GetLotteryByID", { id: _this.data("id") }, function (data) {
                var model = data.model;
                Model.AutoID = model.AutoID;
                Model.CPCode = model.CPCode;
                Model.CPName = model.CPName;
                Model.IconType = model.IconType;
                //Model.Sort = model.Sort;
                Model.ResultUrl = model.ResultUrl;
                _self.createModel();
            });
        });
    }
    //添加/编辑弹出层
    ObjectJS.createModel = function () {
        var _self = this;

        doT.exec("template/SysSet/lottery-detail.html", function (template) {
            var html = template([]);
            Easydialog.open({
                container: {
                    id: "show-model-detail",
                    header: Model.AutoID<1 ? "新建彩种" : "编辑彩种",
                    content: html,
                    yesFn: function () {
                        if (!VerifyObject.isPass()) {
                            return false;
                        }
                        Model.AutoID = $("#modelAutoID").val();
                        Model.CPCode = $("#modelCPCode").val();
                        Model.CPName = $("#modelCPName").val();
                        Model.IconType = $("#modelIconType").val();
                        Model.ResultUrl = $("#modelResultUrl").val(); 
                        _self.saveModel(Model);
                    },
                    callback: function () {

                    }
                }
            });
            VerifyObject = Verify.createVerify({
                element: ".verify",
                emptyAttr: "data-empty",
                verifyType: "data-type",
                regText: "data-text"
            });
            if (Model.AutoID == -1) {
                $("#modelCPCode").focus();
                $("#modelCPCode").removeAttr('redeonly');
                $("#modelCPName").removeAttr('redeonly');
            } else {
                $("#modelCPCode").attr('redeonly', 'redeonly');
                $("#modelCPName").attr('redeonly', 'redeonly');
                $("#modelResultUrl").focus();
            } 
            $("#modelCPCode").val(Model.CPCode);
            $("#modelAutoID").val(Model.AutoID);
            $("#modelCPName").val(Model.CPName);
            $('#modelResultUrl').val(Model.ResultUrl);
            $("#modelIconType").val(Model.IconType);

        });
    }
    //获取列表
    ObjectJS.getList = function () {
        var _self = this;
        $(".tr-header").nextAll().remove();
        $(".tr-header").after("<tr><td colspan='10'><div class='data-loading'><div></td></tr>");
        Global.post("/SysSet/GetLotterys", {}, function (data) {
            _self.bindList(data.items);
        });
    }
    //加载列表
    ObjectJS.bindList = function (items) {
        var _self = this;
        $(".tr-header").nextAll().remove();

        if (items.length > 0) {
            doT.exec("template/SysSet/lottery.html", function (template) {
                var innerhtml = template(items);
                innerhtml = $(innerhtml); 
                innerhtml.find(".setpermission").each(function () {
                    if ($(this).data("type") == 1) {
                        $(this).remove();
                    }
                });

                //绑定启用插件
                innerhtml.find(".status").switch({
                    open_title: "点击销售",
                    close_title: "点击顶手",
                    value_key: "value",
                    change: function (data, callback) {
                       // _self.editStatus(data, data.data("id"), data.data("value"), callback);
                        confirm("暂停销售后用户表不可购买此彩种,确认暂停销售吗？", function () {
                            _self.deleteModel(data.data("id"), data.data("value") ? 0 : 1, function (status) { 
                                if (status) {
                                    _self.getList();
                                } else {
                                    alert("执行失败请联系管理员");
                                }
                            });
                        });
                    }
                });
                //操作
                innerhtml.find(".dropdown").click(function () {
                    var _this = $(this);
                    //if (_this.data("type") != 1) {
                        var position = _this.find(".ico-dropdown").position();
                        $(".dropdown-ul li").data("id", _this.data("id"));
                        $(".dropdown-ul").css({ "top": position.top + 20, "left": position.left - 55 }).show().mouseleave(function () {
                            $(this).hide();
                        });
                    //}
                });
                $(".tr-header").after(innerhtml);
            });
        }
        else {
            $(".tr-header").after("<tr><td colspan='10'><div class='noDataTxt' >暂无数据!</div></td></tr>");
        }
    }
    //保存实体
    ObjectJS.saveModel = function (model) {
        var _self = this;
        Global.post("/SysSet/SaveLottery", { entity: JSON.stringify(model) }, function (data) {
            if (data.model.AutoID > 0) {
                _self.getList();
            }
        })
    }
    //删除
    ObjectJS.deleteModel = function (id,status, callback) {
        Global.post("/SysSet/DeleteLottery", { autoid: id, status: status ,}, function (data) {
            !!callback && callback(data.status);
        });
    }

    //绑定权限页样式
    ObjectJS.initPermission = function (roleid, permissions, menus) {
        var _self = this;
        permissions = JSON.parse(permissions.replace(/&quot;/g, '"'));

        menus = JSON.parse(menus.replace(/&quot;/g, '"'));

        _self.bindMenu(permissions, menus);

        _self.bindPermissionEvent(roleid);


    }

    ObjectJS.bindPermissionEvent = function (roleid) {
        $("#savePermission").click(function () {
            var menus = "";
            $("#rolePermission input").each(function () {
                if ($(this).prop("checked")) {
                    menus += $(this).data("id") + ",";
                }
            });
            Global.post("/SysSet/SaveRolePermission", {
                roleid: roleid,
                permissions: menus
            }, function (data) {
                if (data.status) {
                    alert("角色权限设置成功！");
                } else {
                    alert("角色权限设置失败！");
                }
            });
        });
    }

    ObjectJS.bindMenu = function (permissions, menus) {
        var _self = this;
        for (var i = 0, j = permissions.length; i < j; i++) {
            var menu = permissions[i];
            cacheMenu[menu.MenuCode] = menu.ChildMenus;
        }

        doT.exec("template/SysSet/permissions.html", function (template) {
            var innerHtml = template(permissions);
            innerHtml = $(innerHtml);

            $("#rolePermission").append(innerHtml);

            innerHtml.find("input").change(function () {
                var _this = $(this); 
                if (_this.prop("checked") ) {
                    _this.parent().addClass("checked").removeClass("check").removeClass("checknotall");
                    $("#" + _this.data("id")).find("input").prop("checked", _this.prop("checked"));
                    $("#" + _this.data("id")).find("label").addClass("checked").removeClass("check").removeClass("checknotall");
                } else {
                    _this.parent().addClass("check").removeClass("checked").removeClass("checknotall");
                    $("#" + _this.data("id")).find("input").prop("checked", _this.prop("checked"));
                    $("#" + _this.data("id")).find("label").addClass("check").removeClass("checked").removeClass("checknotall");
                }
                _self.checkRefresh(_this.parent().data("pcode"));
            });

            //默认选中拥有权限
            innerHtml.find("input").each(function () {
                var _this = $(this);
                for (var i = 0, j = menus.length; i < j; i++) {
                    if (_this.data("id") == menus[i].MenuCode) {
                        _this.prop("checked", true);
                        _this.parent().addClass("checked").removeClass("check");
                    }
                }
            });

            innerHtml.find(".openchild").each(function () {
                var _this = $(this);
                var _obj = _self.getChild(_this.attr("data-id"), _this.prevUntil("div").html(), _this.attr("data-eq"), menus);
                _this.parent().after(_obj);
                _this.on("click", function () {
                    if (_this.attr("data-state") == "close") {
                        _this.attr("data-state", "open");
                        _this.removeClass("icoopen").addClass("icoclose");

                        $("#" + _this.attr("data-id")).show();

                    } else { //隐藏子下属
                        _this.attr("data-state", "close");
                        _this.removeClass("icoclose").addClass("icoopen");
                        $("#" + _this.attr("data-id")).hide();
                    }
                });
                if (_self.getClass(_this.data("id")) != "") {
                    $('label[data-cid="' + _this.data("id") + '"]').removeClass("check").removeClass("checked").removeClass("checknotall").addClass(ObjectJS.getClass(_this.data("id")));
                    _self.checkRefresh(_this.data("id"));
                }
            });

        });
    }

    //展开下级
    ObjectJS.getChild = function (menuCode, provHtml, isLast, menus) {
        var _self = this;
        var _div = $(document.createElement("div")).attr("id", menuCode).addClass("hide").addClass("childbox");
        for (var i = 0; i < cacheMenu[menuCode].length; i++) {
            var _item = $(document.createElement("div")).addClass("menuitem");

            //添加左侧背景图
            var _leftBg = $(document.createElement("div")).css("display", "inline-block").addClass("left");
            _leftBg.append(provHtml);
            if (isLast == "last") {
                _leftBg.append("<span class='null left'></span>");
            } else {
                _leftBg.append("<span class='line left'></span>");
            }
            _item.append(_leftBg); 
            //是否最后一位
            if (i == cacheMenu[menuCode].length - 1) {
                _item.append("<span class='lastline left'></span>");

                //加载显示下属图标和缓存数据
                if (cacheMenu[menuCode][i].ChildMenus && cacheMenu[menuCode][i].ChildMenus.length > 0) { 
                    _item.append("<span data-id='" + cacheMenu[menuCode][i].MenuCode + "' data-eq='last' data-state='close' class='icoopen openchild left'></span>");
                    if (!cacheMenu[cacheMenu[menuCode][i].MenuCode]) {
                        cacheMenu[cacheMenu[menuCode][i].MenuCode] = cacheMenu[menuCode][i].ChildMenus;
                    }
                }
            } else {
                _item.append("<span class='leftline left'></span>");

                //加载显示下属图标和缓存数据
                if (cacheMenu[menuCode][i].ChildMenus && cacheMenu[menuCode][i].ChildMenus.length > 0) { 
                    _item.append("<span data-id='" + cacheMenu[menuCode][i].MenuCode + "' data-eq='' data-state='close' class='icoopen openchild left'></span>");
                    if (!cacheMenu[cacheMenu[menuCode][i].MenuCode]) {
                        cacheMenu[cacheMenu[menuCode][i].MenuCode] = cacheMenu[menuCode][i].ChildMenus;
                    }
                }
            }

            _item.append("<label class='check left' data-pcode='" + cacheMenu[menuCode][i].PCode + "' data-cid='"+cacheMenu[menuCode][i].MenuCode+"'><input type='checkbox' class='left'  value='" + cacheMenu[menuCode][i].MenuCode + "'  data-id='" + cacheMenu[menuCode][i].MenuCode + "' /><span>" + cacheMenu[menuCode][i].Name + "</span></label>");

            _div.append(_item);

            _item.find("input").change(function () {
                var _this = $(this); 
                if (_this.prop("checked") ) {
                    $("#" + _this.data("id")).find("input").prop("checked", _this.prop("checked"));
                    $("#" + _this.data("id")).find("label").addClass("checked").removeClass("check").removeClass("checknotall");
                    _this.parents().each(function () {
                        var _parent = $(this);
                        if (_parent.hasClass("childbox")) {
                            _parent.prev().find("input").prop("checked", true);
                            _parent.prev().find("label").addClass("checked").removeClass("check");
                        }
                    }); 
                    _this.parent().addClass("checked").removeClass("check").removeClass("checknotall"); 
                }  else {
                    _this.parent().addClass("check").removeClass("checked").removeClass("checknotall");
                    $("#" + _this.data("id")).find("input").prop("checked", _this.prop("checked"));
                    $("#" + _this.data("id")).find("label").addClass("check").removeClass("checked").removeClass("checknotall");
                }
                _self.checkRefresh(_this.parent().data("pcode"));
            });
            //默认加载下级
            _item.find(".openchild").each(function () {
                var _this = $(this); 
                var _obj = _self.getChild(_this.attr("data-id"), _leftBg.html(), _this.attr("data-eq"), menus);
                _this.parent().after(_obj);
                _this.on("click", function () {
                    if (_this.attr("data-state") == "close") {
                        _this.attr("data-state", "open");
                        _this.removeClass("icoopen").addClass("icoclose");

                        $("#" + _this.attr("data-id")).show();

                    } else { //隐藏子下属
                        _this.attr("data-state", "close");
                        _this.removeClass("icoclose").addClass("icoopen");
                        $("#" + _this.attr("data-id")).hide();
                    }
                });               
            });
        }

        //默认选中拥有权限
        _div.find("input").each(function () {
            var _this = $(this);
            for (var i = 0, j = menus.length; i < j; i++) {
                if (_this.data("id") == menus[i].MenuCode) {
                    _this.prop("checked", true);
                    _this.parent().addClass("checked").removeClass("check");
                }
            }
        }); 
        return _div;
    } 
    ObjectJS.checkRefresh = function (pcode) { 
        if ($("label[data-pcode='" + pcode + "']").length > 0) {
            $("label[data-pcode='" + pcode + "']").each(function (i, v) {
                ObjectJS.checkRefresh($(v).data("cid"));
                var classname = ObjectJS.getClass($(v).data("cid"));
                if (classname != "") {
                    $(v).removeClass("check").removeClass("checked").removeClass("checknotall").addClass(classname);
                }
            });
            var v2 = $("label[data-cid='" + pcode + "']")[0];
            var classname = ObjectJS.getClass($(v2).data("cid"));
            $(v2).removeClass("check").removeClass("checked").removeClass("checknotall").addClass(classname);
        } else {
            return false;
        }
    }
    ObjectJS.getClass = function (divid) { 
        if ($("label[data-pcode='" + divid + "']").length == 0) {
            return "";
        }
        if ($("label[data-pcode='" + divid + "']").hasClass("check") && !$("label[data-pcode='" + divid + "']").hasClass("checked") && !$("label[data-pcode='" + divid + "']").hasClass("checknotall")) {
            return "check";
        } else if ($("label[data-pcode='" + divid + "']").hasClass("check") || $("label[data-pcode='" + divid + "']").hasClass("checknotall")) {
            return "checknotall";
        } else {
            return "checked";
        }
    }
    module.exports = ObjectJS;
});