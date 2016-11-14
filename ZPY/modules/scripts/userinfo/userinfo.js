﻿ 
var regprice = /(^[1-9]*[1-9][0-9]*$)|(^1?\d\.\d$)|(^2[0-3]\.\d$)/;
var regday = /^[1-9]*[1-9][0-9]*$/;
$(function() {     
    /*绑定事件 begin*/
    $('#addUser').click(function () { 
        $(window.parent.document).find("#mainframe").attr('src', '/User/UserAdd');
    }); 
    $('.saleservice').click(function() {
        if (typeof ($(this).attr('checked')) != 'undefined') {
            $(this).removeAttr('checked');
        } else {
            $(this).attr('checked', 'checked');
        }
    });
    $('.myservice').click(function () {
        if (typeof ($(this).attr('checked')) != 'undefined') {
            $(this).removeAttr('checked');
        } else {
            $(this).attr('checked', 'checked');
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
function getUserActions(type,pageindex,pagesize) {
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
                        border: false,
                        text_color: '#79B5E3',
                        background_color: 'none',
                        text_hover_color: '#2573AF',
                        background_hover_color: 'none',
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
function getUserMyInfo() {
    $.post('UserMyInfo', null, function (data) {
        if (data.item != null) { 
            $('.useravator').attr("src", (data.item.Avatar != null && data.item.Avatar != "") ? data.item.Avatar : '/modules/images/head.png');
            $('#BHeight').val(data.item.BHeight);
            $('#BWeight').val(data.item.BWeight); 
            $('#Jobs').val(data.item.Jobs);
            $('#BPay').val(data.item.BPay);
            $('#userAge').val(data.item.Age);
            $('#userTalkTo').val(data.item.TalkTo);
            $('#IsMarry').val(data.item.IsMarry);
            $('#MyContent').val(data.item.MyContent);
            if (data.item.MyService != "" && data.item.MyService != null) {
                $('.myservice').each(function(i, v) {
                    if (data.item.MyService.indexOf($(v).val()) > -1) {
                        $(v).attr('checked', 'checked');
                    }
                });
            }
        }
    });
} 
 
function savaUserNeeds(entity) {
    $.post('/User/SaveNeeds', { entity: JSON.stringify(entity) }, function (data) { 
        if (data.result) {
            alert('发布成功');
            if (entity.type == 0) { 
                $('.adddiary').addClass('hide');
                udedit.setContent('', false);
                getUserDiary(1);
                $('.listdiary').show();

            } else if (entity.type == 2) { 
                $('.saleservice').each(function (i, v) {
                    if ($(v).attr('checked') == 'checked') {
                        $(v).removeAttr('checked');
                    }
                });
                $('#saleprice').val('');
                $('#saletitle').val('');
                $('#salecontent').val('');
                $('.saleprovice').val('');
                $('.salecity').val('');
            } else {
                $('#needtitle').val('');
                $('#needsdays').val('');
                $('#needsprice').val('');
                $('.needsprovice').val('');
                $('.needscity').val('');
                $('#needscontent').val(''); 
            }
        }
    }, "json");
} 

function getconteinfo(replyid,_this) {
    $.post('/Help/GetReplyInfo', { replyid: replyid }, function (data) {
        if (data.result) {
            _this.parent().html(data.errorMsg);
        } else {
            alert(data.errorMsg);
        }
    });
}

function deleteReply(replyid,type) {
    $.post('/Help/DeleteReply', { replyid: replyid }, function (data) {
        if (data.result) {
            alert('删除成功');
            getReplyList(1, type);
        } else {
            alert('操作失败，请稍后再试');
        }
    });
}

function SavaReply(userid, fromuid,replyid) {
    if ($('#replycontent').val() == '') {
        return false;
    }
    var item = {
        GUID: userid,
        Content: $('#replycontent').val(),
        Type: 1,
        FromReplyID: replyid,
        FromReplyUserID: fromuid
    }
    $.post('/Help/SaveReply', { entity: JSON.stringify(item) },
    function (data) {
        if (data.result) {
            $('#replycontent').val('');
            $('#replydialog').hide();
            alert('提交成功');
        } else {
            alert(data.errorMsg);
            location.href = '/Home/Login';
        }
    });
}; 