var reg = /^[1-9]*[1-9][0-9]*$/; 
var receivers = "";
$(function () {
    $('#receivelist option').dblclick(function() {
        if (receivers.indexOf($(this).val()) == -1) {
            receivers += $(this).val() + ",";
            var html = '<div id="id_' + $(this).val() + '" data-id="' + $(this).val() + '" class="receiver" onclick="removed(\'' + $(this).val() + '\');">' + $(this).text() + '<span class="delete-icon" title="点击移除"></span></div>';
            $('#receivediv').append(html);
        } else {
            alert('收件人已存在,不能重复添加!');
        }
    });
    $('#send-message').click(function () {
        var receivers = '';
        $('.receiver').each(function(i, v) {
            receivers += $(v).data('id') + ",";
        });
        if (receivers != "") {
            if ($('#receive_name').val() != '') {
                repModel.FromReplyID = '';
                repModel.FromReplyUserID = '';
                repModel.GuID = receivers;
                repModel.Content = $('#receive_content').val();
                repModel.Title = $('#receive_name').val();
                SavaReply();
            } else {
                alert('标题为空,发送失败');
            }
        } else {
            alert('收件人为选择,发送失败');
            return false;
        }
    });
}); 
var  Params= {type:0,pageIndex:1}

function removed(id) { 
    $('#id_' + id).remove();
    receivers=receivers.replace(id + ',', '');
}

function GetReplay() {
    $.post('/User/GetMsgList', Params,
    function (data) {
        var html = '';
        if (data.items!=null && data.items.length > 0) {
            for (var i = 0; i < data.items.length; i++) {
                html += '<tr><td style="width: 40px; text-align: left;"><input type="checkbox" class="check" style="margin-left: 12px;" data-id="'+ data.items[i].AutoID+'" name="check"/> </td>' +
                    '<td><a href="">' + data.items[i].Title + '</a></td><td><a href="">' + data.items[i].UserName + '</a></td><td>' + convertdate(data.items[i].CreateTime,true) + '</td></tr>';
            }
            
        } else {
            html += '<tr><td colspan="4" style="text-align: center;">暂无数据</td></tr> ';
        }
        $('#sxmsg' + Params.type).html(html);
        $('.pws_tabs_list').css("height", (106 + $('#sxmsg' + Params.type + " tr").length * 41)+'px');
        $("#pager" + Params.type).paginate({
            total_count: data.totalCount,
            count: data.pageCount,
            start: Params.pageIndex,
            display: 5,
            border: true,
            rotate: true,
            images: false,
            mouse: 'slide',
            onChange: function(page) {
                Params.pageIndex = page;
                GetReplay();
            }
        });
    });
}

var repModel = { 
    GuID: '',
    Content: '',
    Title: '',
    FromReplyID: '',
    FromReplyUserID: '',
    Type:0
}

function SavaReply() {
    if (repModel.Title == '' || repModel.GuID=='') {
        return false;
    } 
    $.post('/User/SaveReply', { entity: JSON.stringify(repModel) },
    function (data) {
        if (data.result) {
            $('.receive_content').val(''); 
            alert('提交成功');
        } else {
            alert(data.ErrMsg); 
        } 
    });
};
 
 
