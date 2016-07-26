$(document).ready(function () {
    var service = new WebApplication_WorkFlow01.WebService.WebService1();
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_pageLoaded(PageLoadedHandler);
    function PageLoadedHandler(sender, args) {
        var matterName = $("#mainContent_left_top li:first").text();
        $("#mainContent_right_title").html(matterName);
        $("#HiddenField1").val(matterName);
        service.GetMatterFlow(matterName,
            function (result) {
                drawFlow(result);
            },
            function (error) { $("#mainContent_right_flow").html("Service Error: " + error.get_message()); }
            );
        service.GetMatterAttachment(matterName,
            function (result) {
                if (result != "" && result != "查询附件失败") {
                    $("#attachment").html(result);
                    $("#LinkButton1").css("visibility", "visible");
                    $("#LinkButton2").css("visibility", "visible");
                }
                else {
                    $("#attachment").html("没有附件");
                    $("#LinkButton1").css("visibility", "hidden");
                    $("#LinkButton2").css("visibility", "hidden");
                }
            },
            function (error) { alert("Service Error: " + error.get_message()); }
            );
    }
    $("#mainContent_left_top li").click(function () {
        var matterName = $(this).text();
        $("#mainContent_right_title").html(matterName);
        //alert(matterName);
        $("#HiddenField1").val(matterName);
        service.GetMatterFlow(matterName,
            function (result) {
                drawFlow(result);
            },
            function (error) { $("#mainContent_right_flow").html("Service Error: " + error.get_message()); }
            );
        service.GetMatterAttachment(matterName,
            function (result) {
                if (result != "" && result != "查询附件失败") {
                    $("#attachment").html(result);
                    $("#LinkButton1").css("visibility", "visible");
                    $("#LinkButton2").css("visibility", "visible");
                }
                else {
                    $("#attachment").html("没有附件");
                    $("#LinkButton1").css("visibility", "hidden");
                    $("#LinkButton2").css("visibility", "hidden");
                }
            },
            function (error) { alert("Service Error: " + error.get_message()); }
            );
    });
});
function drawFlow(result) {
    //$("#mainContent_right_flow").html(result);
    var s1 = result;
    //alert(s1); //s1仅用来看是否获取了正确的字符串
    //绘制
    var c = document.getElementById("myCanvas");
    if (c.getContext != null) {
        var ctx = c.getContext('2d');
        //首先清空画布以前的内容
        ctx.fillStyle = "#FFFFFF";
        ctx.fillRect(0, 0, 570, 1200);
        ctx.fillStyle = "#000000";
        if (result == "") return;
        var s2 = result.split(";");
        var x1 = 25, y1 = 25, radius = 15, length = 200, height = 70;
        for (var v in s2) {
            //alert(s2[v]); //看分割的字符串是否正确
            var s3 = s2[v].split(","); //步骤号,步骤描述,部门名称,部门地点
            //以下是绘制圆角矩形框的代码
            ctx.beginPath();
            ctx.strokeStyle = "rgb(67,188,237)";
            ctx.arc(x1, y1, radius, 1.5 * Math.PI, 1.0 * Math.PI, true);
            y1 = y1 - radius;
            ctx.moveTo(x1, y1);
            x1 = x1 + length;
            ctx.lineTo(x1, y1);
            y1 = y1 + radius;
            ctx.arc(x1, y1, radius, 1.5 * Math.PI, 2.0 * Math.PI, false);
            x1 = x1 + radius;
            ctx.moveTo(x1, y1);
            y1 = y1 + height;
            ctx.lineTo(x1, y1);
            x1 = x1 - radius;
            ctx.arc(x1, y1, radius, 0, 0.5 * Math.PI, false);
            y1 = y1 + radius;
            ctx.moveTo(x1, y1);
            x1 = x1 - length;
            ctx.lineTo(x1, y1);
            y1 = y1 - radius;
            ctx.arc(x1, y1, radius, 0.5 * Math.PI, Math.PI, false);
            x1 = x1 - radius;
            ctx.moveTo(x1, y1);
            y1 = y1 - height;
            ctx.lineTo(x1, y1); //此时(x1,y1)=(20,25)
            ctx.stroke();
            //绘制文本
            var x2 = x1 + radius + length / 3, y2 = y1;
            ctx.textBaseline = "top";
            ctx.textAlign = "left";
            ctx.font = "20px '宋体'";
            ctx.fillText("第" + s3[0] + "步", x2, y2);
            x2 = x1 + radius + length / 4, y2 = y1 + height / 3;
            ctx.font = "30px '宋体'";
            ctx.fillText(s3[2], x2, y2);
            //绘制注释框
            length = length + 20;
            height = height + 20;
            x2 = x1 + radius * 2 + length + 20;
            y2 = y1 + height / 2;
            ctx.strokeStyle = "rgb(0,255,0)";
            ctx.beginPath();
            ctx.moveTo(x2, y2);
            ctx.quadraticCurveTo(x2 + 10, y2 + 14, x2 + 40, y2 + 8);
            ctx.quadraticCurveTo(x2 + 40, y2 - height / 2 - radius, x2 + 40 + radius + length / 2, y2 - height / 2 - radius);
            ctx.quadraticCurveTo(x2 + 40 + length + radius, y2 - height / 2 - radius, x2 + 40 + length + radius, y2);
            ctx.quadraticCurveTo(x2 + 40 + length + radius, y2 + height / 2 + radius, x2 + 40 + length / 2 + radius, y2 + height / 2 + radius);
            ctx.quadraticCurveTo(x2 + 40, y2 + height / 2 + radius, x2 + 40, y2 + 20);
            ctx.quadraticCurveTo(x2 + 16, y2 + 18, x2, y2);
            ctx.stroke();
            //绘制注释框内的文本
            var s4 = s3[1].split("."); //科室.地点.经办人.备注
            x2 = x2 + 40 + 5;
            y2 = y2 - height / 2 + 5;
            ctx.textBaseline = "top";
            ctx.textAlign = "left";
            ctx.font = "18px '宋体'";
            ctx.fillStyle = "rgb(255,0,0)";
            ctx.fillText("科室：" + s4[0], x2, y2);
            ctx.fillText("地点：" + s4[1], x2, y2 + 20);
            ctx.fillText("经办人：" + s4[2], x2, y2 + 40);
            ctx.fillText("备注：" + s4[3], x2, y2 + 60);
            //绘制箭头
            if (v != (s2.length - 1)) {
                length = length - 20;
                height = height - 20;
                x2 = x1 + radius + length / 2;
                y2 = y1 + height + radius;
                ctx.fillStyle = "black";
                ctx.fillRect(x2, y2, 2, 22); //绘制黑色矩形
                ctx.beginPath();
                ctx.moveTo(x2 - 4, y2 + 22);
                ctx.lineTo(x2 + 6, y2 + 22);
                ctx.lineTo(x2 + 1, y2 + 22 + 5);
                ctx.fill();
            }
            x1 = x1 + radius;
            y1 = y1 + height + radius + 43;
        }
    }
}