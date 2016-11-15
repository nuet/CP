 
var regprice = /(^[1-9]*[1-9][0-9]*$)|(^1?\d\.\d$)|(^2[0-3]\.\d$)/;
var regday = /^[1-9]*[1-9][0-9]*$/;


var userList = {}
userList.Params = {
    orderby: false,
    username: '',
    userid:'',
    accountmin: '',
    accountmax: '',
    clumon: '',
    type: -1,
    rebatemin: '',
    rebatemax: '',
    pageIndex: 1,
    pageSize:15
}
$(function() {     
    /*绑定事件 begin*/
    $('#addUser').click(function () { 
        $(window.parent.document).find("#mainframe").attr('src', '/User/UserAdd');
    });
    $('.search').click(function () {
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

   

    $(document).click(function(e) {
        if (!$(e.target).parents().hasClass("tips-content") && !$(e.target).hasClass("replycz")
            && !$(e.target).hasClass("box-main") && !$(e.target).parents().hasClass("box-main")) {
            $("#replydialog").fadeOut(500);
        }
    });
    $(".content .sidebar ul li").each(function () {
        var _this = $(this);
        _this.click(function() {
            _this.addClass('hover').siblings().removeClass("hover");
            $('.navcontent').hide();
            $('.' + _this.data('value')).show();
        });
    });  
     
    /*绑定事件结束     */
    //getUserMyInfo(); 
}); 
/*获取用户动态*/
userList.getUserActions= function(type,pageindex,pagesize) {
    $.post('/User/UserActions', { type: type, pageIndex: pageindex, pageSize: pagesize }, function (data) {
        if (data.items.length > 0) {
            var html = '';
            if (type == '2') {
                for (var i = 0; i < data.items.length; i++) {
                    var item = data.items[i];
                    html += '<li data-value="' + item.SeeID + '"><a href="/User/UserMsg/' + item.SeeID + '"><img src="' + (item.SeeAvatar != null && item.SeeAvatar != "" ? item.SeeAvatar : "/modules/images/photo4.jpg") + '" width="61" height="73"><br/>' +
                        '<span>' + item.SeeName + '</span><br/><i>' + getdiff(item.CreateTime, true) + '</i>前</a></li>';
                } 
                $('#whocameul').html(html); 
                $('#page1').html('');
                if (data.pageCount > 0) {
                    $('#page1').paginate({
                        count: data.pageCount,
                        start: 1,
                        display: pagesize,
                        //border: false,
                        //text_color: '#79B5E3',
                        //background_color: 'none',
                        //text_hover_color: '#2573AF',
                        //background_hover_color: 'none',
                        images: false,
                        mouse: 'press',
                        onChange: function(page) {
                            getUserActions(type, page, pagesize);
                        }
                    });
                }
            } else {
                for (var i = 0; i < data.items.length; i++) {
                    var item = data.items[i];
                    html += '<li>' + (item.LeveID != "" ? item.LeveID : "")
                        + '<span><a href="/User/UserMsg/' + item.UserID + '">' + item.UserName + '</a></span>'
                        + (item.Type == 1 ? "发表日志" : item.Type == 2 ? "查看" : item.Type == 3 ? "浏览" : "向") +
                        '<span><a href="/User/UserMsg/' + item.UserID + '">' + item.SeeName + '</a></span>'
                        + item.Remark + '<small><span>' + getdiff(item.CreateTime, true)
                        + '</span>前</small></li>';
                }
                $('#actionul').html(html);
            }
            
        }
    }); 
} 
 
/*获取用户信息*/
userList.UserInfoList = function () {
    userList.Params.username = $('#username').val();
    userList.Params.accountmin = $('#accountmin').val();
    userList.Params.accountmax = $('#accountmax').val();
    userList.Params.clumon = $('#sortby').val();
    userList.Params.type = $('#type').val();
    userList.Params.rebatemin = $('#ratemin').val();
    userList.Params.userid = $('#UserID').val();
    userList.Params.rebatemax = $('#ratemax').val();
    $.post('UserInfoList', userList.Params, function (data) {
        var html = '';
        if (data.items.length > 0) {
            for (var i = 0; i < data.items.length; i++) {
                var item = data.items[i];
                html += '<tr><td class="' + (item.ChildCount > 0 ? "colorblue" : "") + '" data-id="'+item.UserID+'">' + item.UserName + '</td><td>' + (item.Type == 1 ? "代理用户" : "会员用户") + '</td><td>' + item.AccountFee + '</td><td>' + item.Rebate + '</td><td>' + convertdate(item.CreateTime, true) + '</td><td>关闭</td><td>' + (item.SourceType == 0 ? "手动" : "自动") + '</td><td>删除</td></tr>';
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
   
 
 