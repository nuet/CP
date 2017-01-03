var records = {};
var Params = {
    btime: '',
    etime: '', 
    pageIndex: 1
};


$(function () {
    $('.search').click(function () {
        records.getParams();
        if (Params.btime == '') {
            alert('请选择开始时间!');
        } else {
            records.getRecords();
        }
    }); 
});

records.getParams = function () { 
    Params.btime = $('#btime').val();
    Params.etime = $('#etime').val(); 
}
records.getRecords = function () {
    
    $.post('/Report/GetUserReport', Params,
    function (data) {
        var html = '';
        if (data.items != null && data.items.length > 0) {
            for (var i = 0; i < data.items.length; i++) {
                var item = data.items[i];
                html += '<tr><td>' + item.LCode + '</td><td>' + item.UserName + '</td><td>' + convertdateTostring(item.CreateTime, true, 'yyyy-MM-dd hh:mm:ss') + '</td><td>' + item.CPName + '</td><td>' + item.TypeName + '</td>' +
                    '<td>' + item.IssueNum + '</td><td>' + item.Content + '</td><td>' + item.PMuch + '</td><td>元</td><td>' + item.PayFee + '</td><td>' + item.WinFee + '</td><td>' + item.ResultNum + '</td>' +
                    '<td>' + (item.Status == 0 ? "未开奖" : (item.Status == 1 ? "已中奖" : (item.Status == 2 ? "未中奖" : (item.Status == 3 ? "已撤单" : "已删除")))) + '</td></tr>';
            }
        } else {
            html += '<tr><td height="37" colspan="13" style="text-align: center;" class="needq"><span>暂无记录</span></td></tr>';
        }
        $('.data-table tbody').html(html);
        $("#pager").paginate({
            total_count: data.totalCount,
            count: data.pageCount,
            start: Params.pageIndex,
            display: 5,
            border: true,
            rotate: true,
            images: false,
            mouse: 'slide',
            onChange: function (page) {
                Params.pageIndex = page;
                records.getRecords();
            }
        });
    });
}