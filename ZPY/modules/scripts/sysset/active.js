define(function (require, exports, module) {
    var Global = require("global"),
        doT = require("dot"),
        Verify = require("verify"), VerifyObject, editor,
        Easydialog = require("easydialog");

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
            $(window.parent.document).find("#mainframe").attr('src', '/SysSet/ActiveAdd/');
        });
        //删除
        $("#deleteObject").click(function () {
            var id = $(this).data("id");
            confirm("活动删除后不可恢复,确认删除吗？", function() {
                _self.deleteModel(id, function(status) {
                    if (status == 1) {
                        _self.getList();
                    } else {
                        alert('删除失败，请联系管理员');
                    }
                });
            });
        });
        //编辑
        $("#updateObject").click(function () {
            var _this = $(this);
            $(window.parent.document).find("#mainframe").attr('src', '/SysSet/ActiveAdd/' + _this.data("id")); 
        });
    } 
    //获取列表
    ObjectJS.getList = function () {
        var _self = this;
        $(".tr-header").nextAll().remove();
        $(".tr-header").after("<tr><td colspan='6'><div class='data-loading'><div></td></tr>");
        Global.post("/SysSet/GetRoles", {}, function (data) {
            _self.bindList(data.items);
        });
    }
    //加载列表
    ObjectJS.bindList = function (items) {
        var _self = this;
        $(".tr-header").nextAll().remove();

        if (items.length > 0) {
            doT.exec("template/SysSet/roles.html", function (template) {
                var innerhtml = template(items);
                innerhtml = $(innerhtml);
                //操作
                innerhtml.find(".dropdown").click(function () {
                    var _this = $(this);
                    if (_this.data("type") != 1) {
                        var position = _this.find(".ico-dropdown").position();
                        $(".dropdown-ul li").data("id", _this.data("id"));
                        $(".dropdown-ul").css({ "top": position.top + 20, "left": position.left - 55 }).show().mouseleave(function () {
                            $(this).hide();
                        });
                    }
                });

                $(".tr-header").after(innerhtml);
            });
        }
        else {
            $(".tr-header").after("<tr><td colspan='8'><div class='noDataTxt' >暂无数据!</div></td></tr>");
        }
    } 
    //删除
    ObjectJS.deleteModel = function (id, callback) {
        Global.post("/SysSet/DeleteRole", { roleid: id }, function (data) {
            !!callback && callback(data.status);
        });
    }

    //绑定权限页样式
    ObjectJS.initAdd = function (actid,Editor) {
        var _self = this; 
        editor = Editor;
        _self.bindActiveAdd(actid);
    }
    ObjectJS.bindActiveAdd = function(actid) {
        
    }

    module.exports = ObjectJS;
});