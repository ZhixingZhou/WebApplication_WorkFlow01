//刚开始无法获取HiddenField1的值是因为选择器不对，HiddenField1和母版页合在一起后名字被换了
$(document).ready(function () {
    var service = new WebApplication_WorkFlow01.WebService.WebService1();
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_pageLoaded(PageLoadedHandler);
    prm.add_endRequest(EndRequestHandler);
    function PageLoadedHandler(sender, args) {
        var matterName = $("#ContentPlaceHolder1_HiddenField1").val();
        if (matterName == "0") return;
        $("#div1").css("display", "block");
        service.GetMatterFlow(matterName,
        function (result) {
            var s1 = result;
            var s2 = result.split(";");
            //绘制
            var c = document.getElementById("myCanvas");
            if (c.getContext != null) {
                var ctx = c.getContext('2d');
                //首先清空画布以前的内容
                ctx.fillStyle = "#FFFFFF";
                ctx.fillRect(0, 0, 380, 600);
                ctx.fillStyle = "#000000";
                var x1 = 70, y1 = 70, radius = 15, length = 150, height = 50;
                for (var v in s2) {
                    var s3 = s2[v].split(","); //步骤号,步骤描述,部门名称,部门地点
                    //以下是绘制圆角矩形框的代码
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
                    x2 = x1 + radius + length / 4, y2 = y1 + height / 3 + 5;
                    ctx.font = "25px '宋体'";
                    ctx.fillText(s3[2], x2, y2);
                    //绘制箭头
                    if (v != (s2.length - 1)) {
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
        },
        function (error) { alert(error.get_message()); }
        );
    }
    function EndRequestHandler(sender, args) {
        var h = $("#ContentPlaceHolder1_HiddenField1").val();
    }
});