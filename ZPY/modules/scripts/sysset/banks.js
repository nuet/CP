define(function (require, exports, module) {
    var Global = require("global"),
        doT = require("dot"),
        Verify = require("verify"), VerifyObject,
        Easydialog = require("easydialog");
    require("pager");
    var Model = {},
        Rolelist = [];

    var Paras = {
        keyWords: "",
        pageIndex: 1
    }

    var ObjectJS = {};
    //初始化
    ObjectJS.init = function (rolelist) {
        var _self = this;
        _self.bindEvent();
        _self.getList();
        Rolelist = JSON.parse(rolelist.replace(/&quot;/g, '"'));
    }
    //绑定事件
    ObjectJS.bindEvent = function () {
        var _self = this; 

        $("#createModel").click(function () {
            var _this = $(this);
            Model.AutoID = "";
            Model.CardCode = "";
            Model.BankName = "";
            Model.TrueName = "";
            Model.BankChild = "";
            Model.BankPre = "";
            Model.BankCity = "";
            _self.createModel();
        });  
    }  
    //添加/编辑弹出层
    ObjectJS.createModel = function () {
        var _self = this;
        doT.exec("template/SysSet/user-detail.html", function (template) {
            var html = template([]);
            Easydialog.open({
                container: {
                    id: "show-model-detail",
                    header: !Model.AutoID ? "新建用户" : "编辑用户",
                    content: html,
                    yesFn: function () {
                        console.log(VerifyObject.isPass());
                        if (!VerifyObject.isPass()) {
                            return false;
                        }
                        Model.TrueName = $("#modelTrueName").val();
                        Model.CardCode = $('#modelCardCode').val();
                        Model.BankName = $("#modelBankName").val();
                        Model.BankChild = $("#modelBankChild").val();
                        Model.BankPre = $("#modelBankPre").val();
                        Model.BankCity = $("#modelBankCity").val();
                        Model.AutoID = $("#modelAutoID").val();
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
        });
    }

    //获取列表
    ObjectJS.getList = function () {
        var _self = this;
        $(".tr-header").nextAll().remove();
        $(".tr-header").after("<tr><td colspan='4'><div class='data-loading'><div></td></tr>");
        Global.post("/SysSet/GetUsers", Paras, function (data) {
            _self.bindList(data.Items);
            $("#pager").paginate({
                total_count: data.totalCount,
                count: data.pageCount,
                start: Paras.pageIndex,
                display: 5,
                border: true, 
                rotate: true,
                images: false,
                mouse: 'slide',
                onChange: function (page) {
                    Paras.pageIndex = page;
                    _self.getList();
                }
            });
        });
    }

    //加载列表
    ObjectJS.bindList = function (items) {
        var _self = this;
        $(".tr-header").nextAll().remove();
        if (items.length > 0) {
            doT.exec("template/SysSet/banks.html", function (template) {
                var innerhtml = template(items);
                innerhtml = $(innerhtml);
                //操作
                innerhtml.find(".delete").click(function () {
                    var _this = $(this);
                    var id = _this.data("id");
                    confirm("用户解绑后不可恢复,确认解绑吗？", function () {
                        Global.post("/SysSet/DeleteBanks", { id: id }, function (data) {
                            if (data != null && data.status == 1) {
                                _self.getList();
                            } else {
                                alert("操作失败,请重新操作！");
                            }
                        });
                    }); 
                });
                $(".tr-header").after(innerhtml);
            });
        }
        else {
            $(".tr-header").after("<tr><td colspan='4'><div class='noDataTxt' >暂无数据!</div></td></tr>");
        }
    } 
    //保存实体
    ObjectJS.saveModel = function (model) {
        var _self = this;
        Global.post("/SysSet/SaveBanks", { entity: JSON.stringify(model) }, function(data) {
            if (data.errmeg == "执行成功") {
                _self.getList();
            } else {
                alert(data.ErrMsg);
            }
        });
    }

    module.exports = ObjectJS;
});