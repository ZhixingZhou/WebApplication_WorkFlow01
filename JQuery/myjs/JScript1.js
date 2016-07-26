var service = new WebApplication_WorkFlow01.WebService.WebService1();
$(document).ready(function () {
    $(".reply_span").click(function () {
        $(this).next().toggleClass("show_div");
    });
    $(".replyButton").click(function () {
        alert("hello");
        var messageId = $(this).data("messageid");
        var replyUser = $(this).data("replyuser");
        var replyObject = $(this).data("replyobject");
        var objectId = $(this).data("objectid");
        alert(messageId + "," + replyUser + "," + replyObject + "," + objectId);
        var temp = $(this).prev(); //获取当前节点的前一个节点
        var replyContent = temp.text();
        if (replyContent == "" || replyContent == null || replyContent == "请在此输入回复内容") {
            alert("请输入回复内容");
        } else {
            alert(replyContent);
            service.ReplyPublish(messageId, replyContent, replyUser, replyObject,objectId,
            function (result) {
                if (result == true) { alert("发表回复成功！"); window.location.reload(); }
                else { alert("发表回复失败！"); }
            },
            function (error) { alert("Service Error:" + error.get_message()) });
        }
    });
    $("textarea").focus(function () {
        $(this).select();
        $(this).css("color", "#000000");
    });
});