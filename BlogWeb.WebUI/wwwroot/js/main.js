
function getMoreBlog() {
    if ($("#main-content").length) {
        var blogCount = $("#main-content .content-card").length;
        var ajaxRunning = false;
        var blogFind = true;

        $("#content").scroll(function () {
            if ($("#content").scrollTop()+1 >= $("#main-content").height() - $("#content").height() && ajaxRunning == false && blogFind == true) {
                ajaxRunning = true;

                $("#loadSpinner").css("display", "block");

                $.ajax({
                    url: "/Home/GetMoreBlog",
                    data: {
                        blogCounts: blogCount
                    },
                    type: "POST",
                    success: function (result) {
                        var html = '';

                        $.each(result.blogs, function (key, item) {
                            var date = new Date(item.publishedDate);
                            var month = ["Oca", "Şub", "Mar", "Nis", "May", "Haz", "Tem", "Ağu", "Eyl", "Eki", "Kas", "Ara"];
                            html += '\
                                    <div class="col-md-4">\
                                        <div class="content-card">\
                                            <div class="card-image">\
                                                 <img src="/img/'+ item.imageUrl + '" alt="' + item.imageUrl + '">\
                                                 <div class="blogInfo">\
                                                    <div class="infoCat" >\
                                                        <div class="list-item">\
                                                            '+ item.categoryName + '\
                                                        </div>\
                                                        <div class="list-item">\
                                                            <img src="/assets/category.png" width="16" height="16" />\
                                                        </div>\
                                                    </div >\
                                                    <div class="infoHit">\
                                                        <ul>\
                                                            <li>\
                                                                <div class="list-item">\
                                                                    ' + item.visitorHit + '\
                                                                </div>\
                                                                <div class="list-item">\
                                                                    <img src="/assets/eye.png" width="16" height="16" />\
                                                                </div>\
                                                            </li>\
                                                            <li>\
                                                                <div class="list-item">\
                                                                    ' + item.commentCount + '\
                                                                </div>\
                                                                <div class="list-item">\
                                                                    <img src="/assets/comment.png" width="16" height="16" />\
                                                                </div>\
                                                            </li>\
                                                        </ul>\
                                                    </div>\
                                                </div>\
                                            </div>\
                                            <div class="card-title">\
                                                  <a href="/Home/Detail/'+ item.blogId + '" style="text-decoration:none"><h4>' + item.title.toUpperCase() + '</h4></a>\
                                            </div>\
                                            <div class="card-date">\
                                                <span class="dt-info"><span>'+ month[date.getMonth()] + ' ' + date.getDate() + ', ' + date.getFullYear() + '</span></span>\
                                            </div>\
                                            <div class="card-description">\
                                                <p>'+ item.explanation + '</p>\
                                            </div>\
                                            <div class="card-button">\
                                                <a href="/Home/Detail/'+ item.blogId + '">DEVAMINI OKU</a>\
                                            </div>\
                                        </div>\
                                    </div>';
                        });

                        $("#main-content .loadrow").append(html);
                        blogCount = result.newBlogCounts;
                        blogFind = result.blogFind;
                        ajaxRunning = false;
                        $("#loadSpinner").css("display", "none");
                    },
                    error: function (errormessage) {
                        alert(errormessage.statusText);
                    }
                });
            }
        });
    }
}

function getMoreCategoryBlogs(CategoryId) {
    if ($("#main-content").length) {
        var blogCount = $("#main-content .content-card").length;
        var ajaxRunning = false;
        var blogFind = true;

        $("#content").scroll(function () {
            if ($("#content").scrollTop()+1 >= $("#main-content").height() - $("#content").height() && ajaxRunning == false && blogFind == true) {
                ajaxRunning = true;

                $("#loadSpinner").css("display", "block");

                $.ajax({
                    url: "/Home/GetMoreCategoryBlogs",
                    data: {
                        blogCounts: blogCount,
                        categoryId: CategoryId
                    },
                    type: "POST",
                    success: function (result) {
                        var html = '';

                        $.each(result.blogs, function (key, item) {
                            var date = new Date(item.publishedDate);
                            var month = ["Oca", "Şub", "Mar", "Nis", "May", "Haz", "Tem", "Ağu", "Eyl", "Eki", "Kas", "Ara"];
                            html += '\
                                    <div class="col-md-4">\
                                        <div class="content-card">\
                                            <div class="card-image">\
                                                 <img src="/img/'+ item.imageUrl + '" alt="' + item.imageUrl + '">\
                                                 <div class="blogInfo">\
                                                    <div class="infoCat" >\
                                                        <div class="list-item">\
                                                            '+ item.categoryName + '\
                                                        </div>\
                                                        <div class="list-item">\
                                                            <img src="/assets/category.png" width="16" height="16" />\
                                                        </div>\
                                                    </div >\
                                                    <div class="infoHit">\
                                                        <ul>\
                                                            <li>\
                                                                <div class="list-item">\
                                                                    ' + item.visitorHit + '\
                                                                </div>\
                                                                <div class="list-item">\
                                                                    <img src="/assets/eye.png" width="16" height="16" />\
                                                                </div>\
                                                            </li>\
                                                            <li>\
                                                                <div class="list-item">\
                                                                    ' + item.commentCount + '\
                                                                </div>\
                                                                <div class="list-item">\
                                                                    <img src="/assets/comment.png" width="16" height="16" />\
                                                                </div>\
                                                            </li>\
                                                        </ul>\
                                                    </div>\
                                                </div>\
                                            </div>\
                                            <div class="card-title">\
                                                 <a href="/Home/Detail/'+ item.blogId + '" style="text-decoration:none"><h4>' + item.title.toUpperCase() + '</h4></a>\
                                            </div>\
                                            <div class="card-date">\
                                                <span class="dt-info"><span>'+ month[date.getMonth()] + ' ' + date.getDate() + ', ' + date.getFullYear() + '</span></span>\
                                            </div>\
                                            <div class="card-description">\
                                                <p>'+ item.explanation + '</p>\
                                            </div>\
                                            <div class="card-button">\
                                                <a href="/Home/Detail/'+ item.blogId + '">DEVAMINI OKU</a>\
                                            </div>\
                                        </div>\
                                    </div>';
                        });

                        $("#main-content .loadrow").append(html);
                        blogCount = result.newBlogCounts;
                        blogFind = result.blogFind;
                        ajaxRunning = false;
                        $("#loadSpinner").css("display", "none");
                    },
                    error: function (errormessage) {
                        alert(errormessage.statusText);
                    }
                });
            }
        });
    }
}

function ftpostEmailRegistration(e) {
    var emailInput = $(e).parent().find(".email").val();
    var emailInputCount = emailInput.length;
    var control1 = true, control2 = true;
    var thisBtn = $(e);

    if (emailInputCount == 0) {
        control1 = false;
        var html = '\
                            <div class="alert alert-warning alert-dismissible fade show" role = "alert" >\
                                Lütfen önce e-posta adresinizi giriniz!\
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">\
                                    <span aria-hidden="true" >&times;</span >\
                                </button >\
                            </div >';
        $(e).parent().prepend(html);
    }

    var emailControl = new RegExp(/^[^0-9][a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*[@][a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*[.][a-zA-Z]{2,4}$/i);
    if (!emailControl.test(emailInput) && control1 == true) {
        control2 = false;
        var html = '\
                            <div class="alert alert-warning alert-dismissible fade show" role = "alert" >\
                                Bu bir email adresi değil! Lütfen uygun formatta bir email adresi giriniz. (example@example.com)\
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">\
                                    <span aria-hidden="true" >&times;</span >\
                                </button >\
                            </div >';
        $(e).parent().prepend(html);
    }

    if (control1 == true && control2 == true) {
        $.ajax({
            url: "/Home/PostEmailRegistration",
            data: {
                EmailAddress: emailInput
            },
            type: "POST",
            success: function (result) {
                if (result.messageType == 1) {
                    var html = '\
                            <div class="alert alert-warning alert-dismissible fade show" role = "alert" >\
                                '+ result.message + '\
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">\
                                    <span aria-hidden="true" >&times;</span >\
                                </button >\
                            </div >';
                    thisBtn.parent().prepend(html);
                }
                else if (result.messageType == 2) {
                    var html = '\
                            <div class="alert alert-success alert-dismissible fade show" role = "alert" >\
                                '+ result.message + '\
                            </div >';
                    thisBtn.parent().prepend(html);
                    thisBtn.parent().find(".form-group").remove();
                    thisBtn.parent().find("p").remove();
                    thisBtn.remove();
                }
                else if (result.messageType == 3) {
                    var html = '\
                            <div class="alert alert-danger alert-dismissible fade show" role = "alert" >\
                                 '+ result.message + '\
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">\
                                    <span aria-hidden="true" >&times;</span >\
                                </button >\
                            </div >';
                    thisBtn.parent().prepend(html);
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

