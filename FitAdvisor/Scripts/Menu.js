//debugger;
function getMenu() {
    $.ajax({
        url: '/Home/GetUserRole', // Change the URL to match your controller and action
        type: 'GET',
        success: function (data) {
            renderMenu(data);
        }
    });
}

function renderMenu(role) {
    var dynamicMenu = $("#dynamic-menu");
    var rightNav = $("#right-nav");

    dynamicMenu.empty(); // Clear any existing menu items
    var dataUsername = dynamicMenu.attr("data-username");
    var datauserId = dynamicMenu.attr("data-userid");

    if (role === "student") {
        dynamicMenu.append(`
                    <li><a href="/Home/Academic" class="nav-link">Academic</a></li>
                    <li><a href="/Home/Registraion" class="nav-link">Registration</a></li>
                    <li><a href="/Home/Personal" class="nav-link">Personal</a></li>
                    <li><a href="/Home/Instructor" class="nav-link">Instructor</a></li>
                    <li style="padding-left:380px;"">Welcome, <a href="/Personal/StudentInformation/${datauserId}" class="btn btn-success"> ${dataUsername}</a></li>
                    <li style="padding-left:5px;"><a class="btn btn-danger" href="/Home/LogOut" class="nav-link">LogOut</a></li>
        `);
    } else if (role === "instructor") {
        dynamicMenu.append(`
                    <li><a href="/Home/Academic" class="nav-link">Academic</a></li>
                    <li><a href="/Home/Registraion" class="nav-link">Registration</a></li>
                    <li><a href="/Home/Personal" class="nav-link">Personal</a></li>
                    <li style="padding-left:480px;">Welcome, <a href="/PersonalInstructor/UpdateMyInfomation/${datauserId}" class="btn btn-success"> ${dataUsername}</a></li>
                    <li style="padding-left:5px;"><a class="btn btn-danger" href="/Home/LogOut" class="nav-link">LogOut</a></li>
        `);
    } else if (role === "admin") {
        dynamicMenu.append(`
                    <li><a href="/Home/ManageInstructor" class="nav-link">Manage Instructor</a></li>
                    <li><a href="/Home/ManageStudent" class="nav-link">Manage Student</a></li>
                    <li><a href="/Home/ManageCourses" class="nav-link">Manage Courses</a></li>
                    <li style="padding-left:300px;">Welcome, <a href="" class="btn btn-success"> ${dataUsername}</a></li>
                    <li style="padding-left:5px;"><a class="btn btn-danger" href="/Home/LogOut" class="nav-link">LogOut</a></li>
        `);
    }

}

$(document).ready(function () {
    getMenu();
});
