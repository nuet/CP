var records = {};
var Params = {
    cpcode: '',
    stuta: -1,
    btime: '',
    etime: '',
    type: '',
    isuseNum: '',
    lcode: '',
    usetype: -1,
    selfrange: -1,
    pageIndex:1
};


$(function() {
    records.getRecords();
});

records.getRecords=function() {
    $.post('/Lottery/GetLotteryRecord', Params,
    function (data) {
        var html = '';
        if (data.items != null && data.items.length > 0) {
            for (var i = 0; i < data.items.length; i++) {
                console.log(data.items[i]);
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
                GetReplay();
            }
        });
    });
}