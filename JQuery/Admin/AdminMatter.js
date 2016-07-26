/// <reference path="../jquery-1.7.1.min.js" />
/// <reference path="../jquery-1.7.1-vsdoc.js" />
$(function () {
    var ali = $('.div_container .matter_item ul li');
    var adiv = $('.add_search_div div');
    ali.click(function () {
        $(this).addClass('current').siblings().removeClass('current');
        var index = $(this).index();
        adiv.eq(index).show().siblings().hide();
    });
    $("#btnAddStep").hide();
    $("#endEdit").hide();
    $("#btnSave").attr('disabled', 'true');
    $("#btnEdit").attr('disabled', 'true');
    //    $("#btnSave").click(function () {
    //        var strId, xs;
    //        for (var ii = 1; ii <= stepSum; ++ii) {
    //            strId = "Text" + ii.toString();
    //            if ($("#" + strId).val() == "") {
    //                alert("请输入第" + ii.toString() + "步的部门名称！");
    //                xs = "";
    //                return;
    //            }
    //            else {
    //                xs += ";";
    //                xs += $("#" + strId).val();
    //            }
    //        }
    //        //不知道为啥弹出undefined
    //        alert(xs);
    //        $("#btnSave").attr('diaabled', 'true');
    //        
    //    });

});
var stepSum = 0;
var isEmpty = true;
var qflcp;
function addTabTitle(stepNum) {
    $("#tabs li").removeClass('current');
    $("#content p").hide();
    $("#tabs").append('<li class="current"><a class="tab" id="step' + stepNum + '" href="#">第' + stepNum + '步</a><a href="#" class="remove">x</a></li>');
    $("#content").append('<p id="step' + stepNum + '_content">部门名称：<select id="Text' + stepNum + '" style="width:150px"></select></select></p>');
    $("#step" + stepNum.toString() + "_content").show();
    $("#Text" + stepNum.toString()).append('<option value ="0">请选择该步涉及到的部门</option> ');
    service.GetDepartmentName(
            function (result) {
                //alert(result.length.toString());
                //var cd = result.length;
                //alert(cd.toString());
                //$("#Text" + stepNum.toString()).attr('length', eval(cd + 1));
                //alert($("#Text" + stepNum.toString()).length.toString());
                //$("#Text" + stepNum.toString()).options.add(new ("请选择该步涉及到的部门", 0));
                for (var i = 0; i < result.length - 1; ++i) {
                    //                    $("#Text" + stepNum.toString()).options.add(new Option(eval(i + 1), result[i]));
                    $("#Text" + stepNum.toString()).append('<option value ="' + eval(i + 1) + '">' + result[i] + '</option> ');
                }
                qflcp = result.length;
                return;
            },
            function (error) {
                return;
            }
            );
    //    alert($("#Text" + stepNum.toString()).get(0).options.length);
    //    alert($("#Text" + stepNum.toString()).get(0).options[2].text);
    //    alert($("#Text"+stepNum.toString()).length.toString());
    //    $("#Text" + stepNum.toString()).append('<option value ="0">请选择</option> ');
    //    $("#Text" + stepNum.toString()).options.add(new Option("asdf", "0"));
    //    $("#Text" + stepNum.toString()).options[$("#Text" + stepNum.toString()).length] = new Option("asdfasdf", 0);
    //    var oOption = document.createElement("OPTION");
    //    oOption.text = "asdfasdf";
    //    oOption.value = 0;
    //    alert("#Text" + stepNum.toString());
    //    $("#Text" + stepNum.toString()).add(oOption);
    //    $("#Text" + stepNum.toString()).options.add(new Option(0, "agdsgfsdfg")); 
    //    service.GetDepartmentName(
    //    function (result) {
    //    var cd = result.length;
    //    alert(cd.toString());
    //    $("#Text" + stepNum.toString()).attr('length', eval(cd + 1));
    //    alert($("#Text" + stepNum.toString()).length.toString());
    //    $("#Text" + stepNum.toString()).options.add(new ("请选择该步涉及到的部门", 0));
    //    for (var i = 0; i < result.length; ++i) {
    //        $("#Text" + stepNum.toString()).options.add(new Option(eval(i + 1), result[i]));
    //    }
    //    return;
    //    },
    //    function (error) {
    //        return;
    //    }
    //    );

};
$("#tabs a.tab").live('mouseover', function () {
    var contentName = $(this).attr("id") + "_content";
    $("#content p").hide();
    $("#tabs li").removeClass("current");
    $("#" + contentName).show();
    $(this).parent().addClass("current");
});
//显示部门流程
var preRow
function DescriptFlow() {
    var strArr;
    var strName;
    service.GetMatterName(
    function (result) {
        strName = result
    }
    );
    service.GetFlowDepartment(
    function (result) {
        if (result == "") {
            stepSum = 0;
            $("#flag").html(strName + ">>该事项还未添加流程！");
            $("#btnEdit").removeAttr('disabled');
            return;
        }
        isEmpty = false;
        $("#flag").html(strName + ">>涉及到的部门流程>>");
        strArr = result.split(';');
        for (var k = 0; k < strArr.length - 1; ++k) {
            addTabTitle(eval(k + 1));
            var ss = "Text" + eval(k + 1).toString();
            alert(strArr[k]); //为什么加上这一句下面的代码才执行，没有这一句下面的代码根本不执行。

            for (var i = 0; i < qflcp; ++i) {
                if ($("#" + ss).get(0).options[i].text == strArr[k]) {
                    $("#" + ss).get(0).options[i].selected = true;
                    break;
                }
            }

            //$("#"+ss).find("option[text='"+strArr[k]+"']").attr("selected", true);
            //$("#" + ss).attr('value', strArr[k]);
            //$("#" + ss).attr('disabled', 'true');
        }
        stepSum = strArr.length - 1;
        $("p select").attr('disabled', 'true');
        $("#btnSave").attr('disabled', 'true');
        $("#btnEdit").removeAttr('disabled');
        
        
    },
    function (error) {
        alert(error.get_message());
    }
   );
};
//编辑按钮
$("#btnEdit").live('click', function () {
    if (isEmpty) {
        stepSum++;
        addTabTitle(stepSum);
    }
    $("#btnEdit").attr('disabled', 'true');
    $("#btnAddStep").show();
    $("#endEdit").show();
    $("p select").removeAttr("disabled");
    $("#btnSave").removeAttr("disabled");

});
//添加步骤
$("#btnAddStep").live('click', function () {
    var sx = "Text" + stepSum.toString();
    if ($("#" + sx).get(0).selectedIndex == 0) {
        alert("请输入部门名称，再添加步骤！");
        return;
    }
    stepSum++;
    addTabTitle(stepSum);
});
//删除步骤
$('#tabs a.remove').live('click', function () {
    if (!($("#btnEdit").attr('disabled'))) {
        return;
    }
    stepSum--;
    var index = $(this).parent().index();
    var tabId = $(this).parent().find(".tab").attr("id");
    var contentName = tabId + "_content";
    $("#" + contentName).remove();
    $(this).parent().remove();
    if ($("#tabs li.current").length == 0 && $("#tabs li").length > 0) {
        var firstTab = $("#tabs li:first-child");
        firstTab.addClass("current");
        var firstTabId = $(firstTab).find("a.tab").attr("id");
        $("#" + firstTabId + "_content").show();
    }

    if ($("#tabs li").length == 0) {
        stepSum = 0;
        $("#flag").html("该事项还未添加流程！");
        $("#btnAddStep").hide();
        $("#btnEdit").attr('disabled', 'false'); //不管用
        return;
    }

    //步骤变化
    var ali = $("#tabs li");
    var stepid, idTab, idContent, idText, idText1;
    for (var i = index; i < stepSum; ++i) {
        idTab = ali.eq(i).find(".tab").attr("id");
        idContent = "step" + eval(i + 1) + "_content";
        idText = "Text" + eval(i + 1);
        idText1 = "Text" + eval(i + 2);
        $("#" + idTab + "_content").attr("id", idContent);
        $("#" + idText1).attr("id", idText);
        stepid = "step" + eval(i + 1);
        ali.eq(i).find(".tab").text("第" + eval(i + 1) + "步");
        ali.eq(i).find(".tab").attr("id", stepid);
    }
});
//$("#btnSave").live('click', function () {
//    var strId;
//    var xs = "";
//    for (var ii = 1; ii <= stepSum; ++ii) {
//        strId = "Text" + ii.toString();
//        //        if ($("#" + strId).val() == "") {
//        //            alert("请输入第" + ii.toString() + "步的部门名称！");
//        //            xs = "";
//        //            return;
//        //        }
//        //        
//        //        else {
//        //            xs += $("#" + strId ).val();
//        //            xs += ";";
//        //                }
//        if ($("#" + strId).get(0).selectedIndex == 0) {
//            alert("请输入第" + ii.toString() + "步的部门名称！");
//            xs = "";
//            return;
//        }
//        else {
//            xs += $("#" + strId + " option:selected").text();
//            xs += ";";
//        }
//    }
//    $("#btnSave").attr('disabled', 'true');
//    $("#btnEdit").removeAttr("disabled");
//    $("p select").attr('disabled', 'true');
//    $("#btnAddStep").hide();
//    service.SaveFlow(xs,
//        function (result) {
//            if (result) {
//                alert("保存成功！");
//                window.location.reload();
//            }
//            else {
//                alert("保存失败！");
//            }
//        });
//});
//    
$("#endEdit").live('click', function () {
    if (confirm("是否保存！") == true) {
        var strId;
        var xs = "";
        for (var ii = 1; ii <= stepSum; ++ii) {
            strId = "Text" + ii.toString();
            //        if ($("#" + strId).val() == "") {
            //            alert("请输入第" + ii.toString() + "步的部门名称！");
            //            xs = "";
            //            return;
            //        }
            //        
            //        else {
            //            xs += $("#" + strId ).val();
            //            xs += ";";
            //                }
            if ($("#" + strId).get(0).selectedIndex == 0) {
                alert("请输入第" + ii.toString() + "步的部门名称！");
                xs = "";
                return;
            }
            else {
                xs += $("#" + strId + " option:selected").text();
                xs += ";";
            }
        }
        $("#btnSave").attr('disabled', 'true');
        $("#btnEdit").removeAttr("disabled");
        $("p select").attr('disabled', 'true');
        $("#btnAddStep").hide();
        service.SaveFlow(xs,
        function (result) {
            if (result) {
                alert("保存成功！");
            }
            else {
                alert("保存失败！");
            }
        });
    }
    else {
        window.location.href = "AdminMatter.aspx";
    }
    $("#endEdit").hide();
}
);