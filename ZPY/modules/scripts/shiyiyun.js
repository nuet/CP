
 var numArry= ["零", "一", "二", "三", "四", "五", "六", "七", "八", "九"];
 var options= [["1782-4%","1861.2-0%"],["297-4%","310.2-0%"],["198-4%","206.8-0%"],["99-4%","103.4-0%"],["19.8-4%","20.68-0%"],["6.6-4%","6.89-0%"],["3.9-4%","4.07-0%"],["9.9-4%","10.34-0%"],["29.7-4%","31.02-0%"],["118-4%","123.28-0%"],["831-4%","867.96-0%"],["138-4%","144.16-0%"],["39.6-4%","41.36-0%"],["14.8-4%","15.46-0%"]];

 $(function(){
 	//选号：
	$(".num-select li div.numbers").find("span").click(function(){
		$(this).toggleClass("clicked");
		getsumnum();
	})
	//全大小奇偶清：
	$(".sel-actions").find("span").click(function(){
		$(this).addClass("clicked").siblings().removeClass("clicked");
		var $numbers=$(this).parent().siblings(".numbers");
		if (!$(this).parent().parent().parent().parent().hasClass("caizhongw")) {
			switch($(this).index()){
	            case 0:$numbers.find("span").addClass("clicked");break;
	            case 1:$numbers.find("span:gt(4)").addClass("clicked");$numbers.find("span:lt(5)").removeClass("clicked");break;
	            case 2:$numbers.find("span:lt(5)").addClass("clicked");$numbers.find("span:gt(4)").removeClass("clicked");break;
	            case 3:$numbers.find("span:even").addClass("clicked");$numbers.find("span:odd").removeClass("clicked");break;
	            case 4:$numbers.find("span:odd").addClass("clicked");$numbers.find("span:even").removeClass("clicked");break;
	            case 5:$numbers.find("span").removeClass("clicked");break;
		    }
		}else{
			switch($(this).index()){
	            case 0:$numbers.find("span").addClass("clicked");break;
	            case 1:$numbers.find("span:gt(2)").addClass("clicked");$numbers.find("span:lt(3)").removeClass("clicked");break;
	            case 2:$numbers.find("span:lt(3)").addClass("clicked");$numbers.find("span:gt(2)").removeClass("clicked");break;
	            case 3:$numbers.find("span:even").addClass("clicked");$numbers.find("span:odd").removeClass("clicked");break;
	            case 4:$numbers.find("span:odd").addClass("clicked");$numbers.find("span:even").removeClass("clicked");break;
	            case 5:$numbers.find("span").removeClass("clicked");break;
		    }
		}
		getsumnum();
	})

	$(".navs").find("li").click(function(){
		if(typeof($(this).data("name"))!='undefined'){
			$(".play-action select option").eq(0).text(options[$(this).data("name")-1][0]);
		    $(".play-action select option").eq(1).text(options[$(this).data("name")-1][1]);
		}
		$(this).addClass("navs-cur").siblings().removeClass("navs-cur");
		$(".numbers span,.sel-actions span").removeClass("clicked");
		$(".play-action .times").val("1");
		$(".play-action p span").text("0");
		switch($(this).index()){
			case 0:$(".n-star").show().siblings("div").hide();
			       
			       $(".n-star .play-section .zhifu").find("p").eq(0).html("从第一位、第二位、第三位中至少各选择1个号码。");
			       $(".n-star .play-section .zhidan").find("p").eq(0).html("手动输入号码，至少输入1个三位数号码组成一注。");
			       $(".n-star .play-section .zufu").find("p").eq(0).html("从01-11中任意选择3个或3个以上号码。");
			       $(".n-star .play-section .zudan").find("p").eq(0).html("手动输入号码，至少输入1个三位数号码组成一注。");
                   if ($(".n-star .zhifu li").eq(2).css("display")=="none") {
			       	   $(".n-star .zhifu li").eq(2).show();
			       }
			       $(".n-star .all-ways ul li").eq(2).data("type","3");
                   $(".play-action .select-fan,.play-action select").show();
                   $(".n-star .all-ways ul li:first-child,.n-star .all-ways ul li:nth-child(2)").data("name","1");
                   $(".n-star .all-ways ul li:nth-child(3),.n-star .all-ways ul li:nth-child(4)").data("name","2");
			       $(".n-star .all-ways ul li:first-child").click();

			    

			break;
			case 1:$(".n-star").show().siblings("div").hide();
			       
			       $(".n-star .play-section .zhifu").find("p").eq(0).html("从第一位、第二位中至少各选择1个号码。");
			       $(".n-star .play-section .zhidan").find("p").eq(0).html("手动输入号码，至少输入1个两位数号码组成一注。");
			       $(".n-star .play-section .zufu").find("p").eq(0).html("从01-11中任意选择2个或2个以上号码。");
			       $(".n-star .play-section .zudan").find("p").eq(0).html("手动输入号码，至少输入1个两位数号码组成一注。");
			       if ($(".n-star .zhifu li").length==3) {
			       	   $(".n-star .zhifu li").eq(2).hide();
			       }
			       $(".n-star .all-ways ul li").eq(2).data("type","2");
			       $(".play-action .select-fan,.play-action select").show();
			       $(".n-star .all-ways ul li:first-child,.n-star .all-ways ul li:nth-child(2)").data("name","3");
                   $(".n-star .all-ways ul li:nth-child(3),.n-star .all-ways ul li:nth-child(4)").data("name","4");
                   $(".n-star .all-ways ul li:first-child").click();

			break;
			case 2:$(".fixed").show().siblings("div").hide();
			       $(".play-action .select-fan,.play-action select").show();break;
			case 3:$(".non-fixed").show().siblings("div").hide();
			       $(".play-action .select-fan,.play-action select").show();break;
			case 4:$(".optional").show().siblings("div").hide();
			       $(".play-action .select-fan,.play-action select").show();
                   $(".optional .all-ways .fushi ul li:first-child").click();
			       break;
			case 5:$(".interests").show().siblings("div").hide();
                   $(".play-action .select-fan,.play-action select").hide();
			break;
		}
	})
	$(".all-ways").find("li").click(function(){
		console.log(1);
		if(typeof($(this).data("name"))!='undefined'){
			$(".play-action select option").eq(0).text(options[$(this).data("name")-1][0]);
		    $(".play-action select option").eq(1).text(options[$(this).data("name")-1][1]);
	    }
		$(".numbers span,.sel-actions span").removeClass("clicked");
		$(".play-action .times").val("1");
		$(".play-action p span").text("0");
		$(this).addClass("all-ways-cur").siblings().removeClass("all-ways-cur");
		$(this).parent().parent().siblings(".play-section").children("div").eq($(this).index()).show();
		$(this).parent().parent().siblings(".play-section").children("div").eq($(this).index()).siblings().hide();

	});
	$(".optional .all-ways .fushi").find("li").click(function(){
		
		// $(this).parent().parent().parent().siblings(".play-section").children("div").eq($(this).index()).show();
		// $(this).parent().parent().parent().siblings(".play-section").children("div").eq($(this).index()).siblings().hide();
		$(".fu1").show();
		$(".dan1").hide();
		$(this).parent().parent().siblings(".danshi").find("li").removeClass("all-ways-cur");

		$('.numtips1').html($(this).data('type'));
		$('.num-select .numtips1').html(numArry[$(this).data('type')]);
		$('.optional .numbers span.clicked,.optional .sel-actions span.clicked').removeClass('clicked'); 
	})
	$(".optional .all-ways .danshi").find("li").click(function(){
		$(this).parent().parent().siblings(".fushi").find("li").removeClass("all-ways-cur");
		$(".fu1").hide();
		$(".dan1").show();
		$('.numtips2').html($(this).data('type'));
		$(".dan1 textarea").val("");
	})
 
	//加1减1：
	$("img[alt='plus']").click(function(){
		var nums=$(".play-action").find("p span:first-child").text();
		var $val=$(this).siblings(".times").val();
		$(this).siblings(".times").val(parseInt($val)+1);
		var times1=$(this).siblings(".times").val();
		$(".play-action").find("p span:last-child").text(nums*times1*2);
	})
	$("img[alt='minus']").click(function(){
		var nums=$(".play-action").find("p span:first-child").text();
		var $val=$(this).siblings(".times").val();
		if ($val>1) {
			$(this).siblings(".times").val($val-1);
			var times2=$(this).siblings(".times").val();
		    $(".play-action").find("p span:last-child").text(nums*times2*2);
		}
	})
	//追号3个导航：
    $(".chase-action>ul li").click(function(){
    	$(this).addClass("chase-action-cur").siblings().removeClass("chase-action-cur");
    	switch($(this).index()){
    		case 0:$(".lrl-chase p:nth-child(2)").html('起始倍数：<img src="images/minus.png" alt="minus" width="27"><input type="text" class="times" value="1"/><img src="images/plus.png" alt="plus" width="27">倍<span>最低收益率：<input type="text" value="50"/>%</span><span>追号期数：</span><input type="text" value="10"/>');break;
    		case 1:$(".lrl-chase p:nth-child(2)").html('起始倍数：<img src="images/minus.png" alt="minus" width="27"><input type="text" class="times" value="1"/><img src="images/plus.png" alt="plus" width="27">倍<span>追号期数：</span><input type="text" value="10"/>');break;
    		case 2:$(".lrl-chase p:nth-child(2)").html('隔&nbsp;<img src="images/minus.png" alt="minus" width="27"><input type="text" class="times" value="1"/><img src="images/plus.png" alt="plus" width="27">期<span>倍数：</span><input type="text" value="2"/><span>追号期数：</span><input type="text" value="10"/>');break;
    	}
    })
    //发起追号：
    $(".chase-num").find("input[type='button']").click(function(){
    	$this=$(this);
    	if ($(".num-selected table tbody tr:first-child").find("td").eq(0).text()!="") {

    	    $this.toggleClass("up");
    		
    		$(".chase-action").toggle();

    		if ($this.hasClass("up")) {
                $this.siblings().find("input[type='checkbox']").prop("checked",true);
    		}else{
    			$this.siblings().find("input[type='checkbox']").prop("checked",false);
    		}
    	}else{
    		$('#basic-dialog-ok').modal({
                "opacity":30
	        });
    		$('#basic-dialog-ok').find(".tips-title span.tip1").text("请先添加投注内容");
	        return false;
    	}
    })
 
	//选号入框：
	$(".play-action").find("input[type='button']").click(function(){
		if ($(this).siblings("p").find("span").eq(0).text()>0) {
	        //
		}else{
			$('#basic-dialog-ok')
	        .modal({
	                "opacity":30
	        });
	        $('#basic-dialog-ok').find(".tips-title span.tip1").text("号码选择不完整，请重新选择");
	        return false;
		}
	})

	//操作已选号：
	$(".num-selected table tbody tr").on({
		click:function(){ 
		    if($(this).find("td").eq(0).text()!=""){
		        //
			}
		},
		mouseover:function (v){
			if($(this).find("td").eq(0).text()!=""){
		        //
			}
		}
	})

	//清空选号：
	$(".num-selected table tfoot tr td a").click(function(){
		$("body").append("<div class='cover-layer'></div>");
        $(".alert1").show().css("z-index","10001");
		$(".alert1 p button").eq(0).click(function(){
			$(".num-selected table tbody").html("<tr><td></td><td></td><td></td><td></td><td>暂无投注项</td><td></td><td></td><td></td><td></td></tr>");
            $(".alert1").hide();
            $(".cover-layer").remove();
			return;
		})
	})

	//弹出框操作：(清空所有，立即投注，)
    $(".alert1 h3 img,.alert1 p button:last-child").click(function(){
    	$(".alert1").hide();
    	$(".cover-layer").remove();
    	if ($(".alert1 p:last-child span").length!=0) {
    		$(".alert1 p:last-child span").remove();
    	}
    	$(".alert1").css({"width":"300px","height":"160px","top":"40%"});
        $(".alert1 h3+p").html('<img src="images/tips.png" width="40"><span>是否清空确认区中所有投注内容？</span>').css("text-align","center");
        $(".alert1 p:last-child span").remove();
        $(".alert1 p:last-child").css({"text-align":"center","margin-top":"-10px"});
        $(".alert1 p button").eq(0).css("margin-left","35px");
    })

    //立即投注：
    $(".add-bet input").click(function(){
    	if ($(".num-selected table tbody tr:first-child").find("td").eq(0).text()=="") {
            $('#basic-dialog-ok').modal({
                "opacity":30
	        });
    		$('#basic-dialog-ok').find(".tips-title span.tip1").text("请先添加投注内容");
	        return false;
    	}else{
    		$("body").append("<div class='cover-layer'></div>");
            $(".alert1").show().css("z-index","10001");
            $(".alert1").css({"width":"464px","height":"286px","top":"35%"});
            $(".alert1 h3+p").html('<img src="images/tips2.png" width="40" style="margin-left:-10px;"><span>你确定加入<strong>16023658</strong>期？</span><br/><textarea style="width:90%;height:100px;margin:0 auto;margin-top:-10px;"></textarea>').css("text-align","center");
            $(".alert1 h3+p textarea").text("4545");
            $(".alert1 p:last-child").prepend('<span class="totle-money">投注总金额<strong style="margin:0 5px;">2400</strong>元</span>');
            $(".alert1 p:last-child").css({"text-align":"center","margin-top":"10px"});
            $(".alert1 p button").eq(0).css("margin-left","45px").click(function(){
				//投注进去：

	            $(".alert1").hide();
	            $(".cover-layer").remove();
	            $(".alert1 p:last-child span").remove();
	            //恢复弹出框原样：
	            $(".alert1").css({"width":"300px","height":"160px","top":"40%"});
	            $(".alert1 h3+p").html('<img src="images/tips.png" width="40"><span>是否清空确认区中所有投注内容？</span>').css("text-align","center");
	            $(".alert1 p:last-child span").remove();
	            $(".alert1 p:last-child").css({"text-align":"center","margin-top":"-10px"});
	            $(".alert1 p button").eq(0).css("margin-left","35px");
			    return;
		    })
    	}
    })



   

})

//外部函数：

function getsumnum(){
	var locdiv='';
	var times=$("img[alt='plus']").siblings(".times").val();

	if(typeof($('.navs-cur').data('id'))!='undefined'){
		locdiv='.'+$('.navs-cur').data('id');
	}

	var zf=typeof($(locdiv+' .all-ways-cur').data('id'))!='undefined'? ' .'+$(locdiv+' .all-ways-cur').data('id'):"";
	
 
     var s=0; 
	if(zf==' .zhifu'){
		// for(var m=0;m<$(locdiv+zf+' .numbers').length;m++){

		// }
		var $span1 =$(locdiv+zf+' .numbers').eq(0).find("span.clicked");
	    var $span2 =$(locdiv+zf+' .numbers').eq(1).find("span.clicked");
	    var $span3 =$(locdiv+zf+' .numbers').eq(2).find("span.clicked");   	   
	    for (var i = 0; i < $span1.length; i++) {
	    	for (var j = 0; j < $span2.length; j++) {
	    		if($(locdiv+zf+' ul li:last-child').css("display")!="none"){
		    		for (var k = 0; k < $span3.length; k++) {
		    			if($span1.eq(i).text()!=$span2.eq(j).text()&&$span1.eq(i).text()!=$span3.eq(k).text()&&$span2.eq(j).text()!=$span3.eq(k).text()){
		                   s++;
		    			}
		    		}
		    	}else{
		    		if($span1.eq(i).text()!=$span2.eq(j).text()){
		                   s++;
	    			}
		    	}
	    	}
	    }
	    // $(locdiv+" .zhifu .play-action").find("p span:first-child").text(s);
	    // $(locdiv+" .zhifu .play-action").find("p span:last-child").text(2*s*times);
	    $(".play-action").find("p span:first-child").text(s);
	    $(".play-action").find("p span:last-child").text(2*s*times);
	}
	else if(zf==' .zufu'){    
		if(typeof($(locdiv+' .all-ways-cur').data('type'))!='undefined'){
			var typelength=$(locdiv+' .all-ways-cur').data('type');
			var sctlen =$(locdiv+zf+' .numbers').eq(0).find("span.clicked").length;
			//s=(sctlen*(sctlen-1)/2 ) *(typelength>2?((sctlen-2)/3):1);
			s=fibonacci(sctlen,typelength);
			// $(locdiv+" .zufu .play-action").find("p span:first-child").text(s);
			// $(locdiv+" .zufu .play-action").find("p span:last-child").text(2*s*times);
			$(".play-action").find("p span:first-child").text(s);
			$(".play-action").find("p span:last-child").text(2*s*times);
		}else{
			console.log('未定义data-type');
		}
	}
    else if(zf==' .fu1'){        
		if(typeof($(locdiv+' .all-ways-cur').data('type'))!='undefined'){
			var typelength=$(locdiv+' .all-ways-cur').data('type');
			var sctlen =$(locdiv+zf+' .numbers').eq(0).find("span.clicked").length;
			//s=(sctlen*(sctlen-1)/2 ) *(typelength>2?((sctlen-2)/3):1);
			s=fibonacci(sctlen,typelength);
			// $(locdiv+" .fu1 .play-action").find("p span:first-child").text(s);
			// $(locdiv+" .fu1 .play-action").find("p span:last-child").text(2*s*times);
			$(".play-action").find("p span:first-child").text(s);
			$(".play-action").find("p span:last-child").text(2*s*times);
		}else{
			console.log('未定义data-type');
		}
	}
	else if(zf==''){
		if (locdiv!=".interests") {           //这是定位与不定位的注数和金额：
			s=$(locdiv+zf+' .numbers').find("span.clicked").length;
			// $(locdiv+" .play-action").find("p span:first-child").text(s);
			// $(locdiv+" .play-action").find("p span:last-child").text(2*s*times);
			$(".play-action").find("p span:first-child").text(s);
			$(".play-action").find("p span:last-child").text(2*s*times);
		}else{                                //这是趣味型的注数和金额：
			if ($(".dingdans").css("display")!="none"){  //订单双的
                s=$(locdiv+' .dingdans .numbers').find("span.clicked").length;
	            // $(locdiv+" .dingdans .play-action").find("p span:first-child").text(s);
	            // $(locdiv+" .dingdans .play-action").find("p span:last-child").text(2*s*times);
	            $(".play-action").find("p span:first-child").text(s);
	            $(".play-action").find("p span:last-child").text(2*s*times);
			}else{  //猜中位的
	            var s1=$(locdiv+' .caizhongw .numbers').find("span.clicked").length;
	            // $(locdiv+" .caizhongw .play-action").find("p span:first-child").text(s1);
	            // $(locdiv+" .caizhongw .play-action").find("p span:last-child").text(2*s1*times);
	            $(".play-action").find("p span:first-child").text(s1);
	            $(".play-action").find("p span:last-child").text(2*s1*times);
            }
		}
	}
} 
 
function fibonacci( a, b){  
    if (a<b) {
    	return 0;
    }else{
        if(b>a/2){  
	        return fibonacci(a,a-b);  
	    }  
	    return up(a,b)/up(b,b);  
    }
}  
  
function up( a, b){  
    var c = 1;  
    for(var i=0;i<b;i++){  
        c = c*a;  
        a--;  
    }  
    return c;  
} 


