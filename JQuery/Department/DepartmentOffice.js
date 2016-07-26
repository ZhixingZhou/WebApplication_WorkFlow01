$(function () {
    var ali = $('.div_container .office_item ul li');
    var adiv = $('.add_search_div div');
    //adiv.addClass('hide');
    ali.click(function () {
        $(this).addClass('current').siblings().removeClass('current');
        var index = $(this).index();
        //adiv.eq(index).removeClass('hide');
        adiv.eq(index).show().siblings().hide();
        //adiv.eq(index).show().siblings().addClass('hide');

    });
});

function IsDeleteOffice(deleteId,departmentId) {
    service.IsHaveMatter(deleteId,departmentId,
    function (result) {
        if (result) {
            if (confirm("有事项涉及到该科室，确定要删除吗？") == true) {
                service.DeleteOffice(deleteId,departmentId,
                function (result) {
                    if (result) {
                        alert("删除成功！");
                        window.location.href = 'DepartmentOffice.aspx';
                    }
                    else {
                        alert("对不起，删除失败了！");
                    }
                },
                function (error) {

                })
            }
        }
        else {
            if (confirm("确定要删除该科室吗？") == true) {
                service.DeleteOffice(deleteId,departmentId,
                function (result) {
                    if (result) {
                        alert("删除成功！");
                        window.location.href = 'DepartmentOffice.aspx';
                    }
                    else {
                        alert("对不起，删除失败了！");
                    }
                },
                function (error) {

                })
            }
        }
    },
    function (error) {

    });
};