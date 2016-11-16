 
var regprice = /(^[1-9]*[1-9][0-9]*$)|(^1?\d\.\d$)|(^2[0-3]\.\d$)/;
var regday = /^[1-9]*[1-9][0-9]*$/;

 
var userList = {}

userList.Params = {
    orderby: false,
    username: '',
    userid: '',
    accountmin: '',
    accountmax: '',
    clumon: '',
    type: -1,
    rebatemin: '',
    rebatemax: '',
    pageIndex: 1,
    pageSize: 15,
    mytype: false
}
$(function() {     
    /*绑定事件 begin*/
    $('#addUser').click(function () { 
        $(window.parent.document).find("#mainframe").attr('src', '/User/UserAdd');
    });
    $('.search').click(function () { 
        userList.Params.userid = $('#UserID').val();
        userList.Params.mytype = false;
        userList.UserInfoList();
    });
    $('#sortbymax').click(function () {
        if (typeof ($(this).attr('checked')) != 'undefined') {
            $(this).removeAttr('checked');
            userList.Params.orderby = false;
        } else {
            $(this).attr('checked', 'checked');
            userList.Params.orderby = true;
        }
    }); 
    userList.UserInfoList();
    userList.getChildList('', '.b_box');
}); 
/*获取用户动态*/
userList.getChildList= function(id,type) {
    $.post('/User/GetChildList', { userid: id, type: (type == ".b_box" ? true : false) }, function (data) {
        if (data.items.length > 0) {
            var html = '';
            for (var i = 0; i < data.items.length; i++) {
                var item = data.items[i]; 
                html += ' <li  data-id="' + item.UserID + '"><a title="点击查看" class="'+(item.ChildCount==0?"no-icon":"")+'" href="javascript:void(0);" data-id="' + item.UserID + '">' + item.LoginName + '</a><span class="' + (item.ChildCount > 0 ? "n_links" : "n") + '" title="' + (item.ChildCount > 0 ? "点击展开" : "") + '" id="span' + item.UserID + '">' + item.ChildCount + '</span>';
            }  
            if (type == '.b_box') {
                $(type).html(html);
                $(type).find('li a').click(function () {
                    $('.b_box').data('id', $(this).parent().data('id'));
                    userList.paramsClear();
                    userList.Params.userid = $(this).parent().data('id');
                    userList.Params.mytype = true;
                    userList.UserInfoList();
                });
                $(type).find('.n_links').click(function () { 
                    if (typeof ($(this).after().html()) == 'undefined' || $(this).after().html() == '') {
                        userList.getChildList($(this).parent().data('id'), '#' + $(this).attr('id'));
                    } else {
                        if ($(this).parent().hasClass('on')) {
                            $(this).parent().removeClass('on');
                            $(this).after().hide();
                        } else {
                            $(this).parent().addClass('on');
                            $(this).after().show();
                        }
                    }
                });
            } else {
                $(type).after().html('<ul id="ext" class="on">' + html + '</ul>');
                $(type).after().find('li a').click(function () {
                    $('.b_box').data('id', $(this).parent().data('id'));
                    userList.paramsClear();
                    userList.Params.userid = $(this).parent().data('id');
                    userList.Params.mytype = true;
                    userList.UserInfoList();
                });
                $(type).after().find('.n_links').click(function () {
                    if (typeof ($(this).after().html()) == 'undefined' || $(this).after().html()=='') {
                        userList.getChildList($(this).parent().data('id'), '#' + $(this).attr('id'));
                    } else {
                        if ($(this).parent().hasClass('on')) {
                            $(this).parent().removeClass('on');
                            $(this).after().hide();
                        } else {
                            $(this).parent().addClass('on');
                            $(this).after().show();
                        }
                    }
                });
            }
        } else {
            $('#leftcontent').hide();

        }
    }); 
} 
userList.paramsClear= function() {
    userList.Params.username = '';
    userList.Params.accountmin = '';
    userList.Params.accountmax = '';
    userList.Params.clumon = '';
    userList.Params.type = -1;
    userList.Params.rebatemin = '';
    userList.Params.rebatemax = '';
    userList.Params.userid = '';
    userList.Params.mytype = false;
}
/*获取用户信息*/
userList.UserInfoList = function () {
    userList.Params.username = $('#username').val();
    userList.Params.accountmin = $('#accountmin').val();
    userList.Params.accountmax = $('#accountmax').val();
    userList.Params.clumon = $('#sortby').val();
    userList.Params.type = $('#type').val();
    userList.Params.rebatemin = $('#ratemin').val();
    userList.Params.rebatemax = $('#ratemax').val();
    $.post('UserInfoList', userList.Params, function (data) {
        var html = '';
        var layers = parseInt($('#Layers').val());
        if (data.items.length > 0) {
            for (var i = 0; i < data.items.length; i++) {
                var item = data.items[i];
                html += '<tr style="'+(item.UserID== $('.b_box').data('id')?"background-color: #F5EDE4;":"")+'"><td class="' + (item.ChildCount > 0 ? "colorblue" : "") + '" data-id="' + item.UserID + '">' + item.UserName + '</td><td>' + (item.Type == 1 ? (parseInt(item.Layers) - layers) + "级代理" : "会员用户") + '</td><td>' + item.AccountFee + '</td><td>' + item.Rebate + '</td><td>' + convertdate(item.CreateTime, true) + '</td><td>关闭</td><td>' + (item.SourceType == 0 ? "手动" : "自动") + '</td><td>删除</td></tr>';
            } 
        } else {
            html = '<tr><td height="37" colspan="13" style="text-align: center;" class="needq"><span>暂无数据</span></td></tr>';
        }
        $('#usertbody').html(html);
        $('#usertbody .colorblue').click(function() {
            $('#UserID').val($(this).data('id'));
            userList.UserInfoList();
        });
        $("#pager").paginate({
            total_count: data.TotalCount,
            count: data.pageCount,
            start: userList.Params.pageIndex,
            display: 5,
            border: true,
            rotate: true,
            images: false,
            mouse: 'slide',
            onChange: function (page) {
                userList.Params.pageIndex= page;
                userList.UserInfoList();
            }
        });
    });
} 
   
 
 