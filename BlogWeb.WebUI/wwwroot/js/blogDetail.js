function getMoreComment() {
    if ($("#detail-content").length) {
        var commentCount = $("#detail-content .comment").length;

        $("#getBeforeComments").on("click", function (e) {
            var blogId = $("#detail-content .BlogId").val();

            $.ajax({
                url: "/Home/GetMoreComment",
                data: {
                    comCount: commentCount,
                    blogid: blogId
                },
                type: "POST",
                success: function (result) {
                    var html = '';

                    if (result.commParentCount > 0) {
                        $("#getBeforeComments").html("Önceki yorumları gör (" + result.commParentCount + ")");
                    } else {
                        $("#getBeforeComments").remove();
                    }

                    $.each(result.commParents, function (key, item) {
                        var date = new Date(item.createTime);

                        html += '\
                                <div class="comment">\
                                    <input class="openAnswerSomeone" type="hidden" value="0" />\
                                    <input class="toNameComment" type="hidden" value="'+ item.fullname + '"/>\
                                    <input class="CommentId" type="hidden" value="'+ item.commentId + '"/>\
                                    <div class="com_name">\
                                        <h3>'+ item.fullname + '</h3>\
                                    </div>\
                                    <div class="com_comment">\
                                        <p>'+ item.text + '</p>\
                                    </div>\
                                    <div class="com_other">\
                                        <a>Yanıtla</a><span>'+ date.getDate() + '.' + (date.getMonth() < 10 ? '0' : '') + (date.getMonth() + 1) + '.' + date.getFullYear() + ' ' + date.getHours() + ':' + (date.getMinutes() < 10 ? '0' : '') + date.getMinutes() + '</span>\
                                    </div>';

                        if (result.commAnswerCount.length > 0) {
                            $.each(result.commAnswerCount, function (key, c) {
                                if (item.commentId == c.parentId && c.answerCount > 0) {
                                    html += '\
                                            <div class="beforeAnswers">\
                                                <a class="getBeforeAnswer">Önceki yanıtları gör ('+ c.answerCount + ')</a>\
                                            </div>';
                                }
                            });
                        }

                        $.each(result.commAnswers, function (key, answer) {
                            var dateAnswer = new Date(answer.createTime);
                            if (item.commentId == answer.parentId) {
                                html += '\
                                        <div class="comment_answer">\
                                            <input class="openAnswerSomeone" type="hidden" value="0" />\
                                            <input class="toName" type="hidden" value="'+ answer.fullname + '"/>\
                                            <div class="com_name">\
                                                <h3>';

                                if (answer.type == "admin") {
                                    html += '\
                                                    <span>\
                                                        <img src="/assets/crown.png" width="14" height="14" title="Yönetici" />\
                                                    </span>';
                                }

                                html += answer.fullname +
                                    '</h3>\
                                            </div>\
                                            <div class="com_comment">';
                                if (answer.answerToName != null) {
                                    html += '<p><span class="answerToName">' + answer.answerToName + ' </span>' + answer.text + '</p>';
                                } else {
                                    html += '<p>' + answer.text + '</p>';
                                }

                                html += '\
                                            </div>\
                                            <div class="com_other">\
                                                <a>Yanıtla</a><span>'+ dateAnswer.getDate() + '.' + (dateAnswer.getMonth() < 10 ? '0' : '') + (dateAnswer.getMonth() + 1) + '.' + dateAnswer.getFullYear() + ' ' + dateAnswer.getHours() + ':' + (dateAnswer.getMinutes() < 10 ? '0' : '') + dateAnswer.getMinutes() + '</span>\
                                            </div>\
                                       </div>';
                            }
                        });

                        commentCount = result.commCount;
                        html += '</div>'; // Closed div for 'comment'
                    });

                    $("#detail-content .blog_comments .beforeComments").after(html);
                    moreLessControl();
                },
                error: function (errormessage) {
                    alert(errormessage.responseText);
                }
            });
        });

    }
}

function getMoreAnswer() {
    if ($("#detail-content").length) {

        // Html ile eklenen bağlantıyı tekrardan jquery ile dinlemeye(click) çalıştığımızda buton çalışmayacaktır.
        // Bu durumda 'delegate' işlemi body'e uygularanarak hata düzeltilebilir.
        $("body").delegate("#detail-content .comment .getBeforeAnswer", "click", function (e) {
            var commentId = $(this).parents(".comment").children(".CommentId").val();
            var answerCount = $(this).parents(".comment").children(".comment_answer").length;
            var thisElement = $(this);

            $.ajax({
                url: "/Home/GetMoreAnswer",
                data: {
                    comId: commentId,
                    ansCount: answerCount
                },
                type: "POST",
                success: function (result) {
                    var html = '';

                    $.each(result.commAnswers, function (key, answer) {
                        var dateAnswer = new Date(answer.createTime);

                        html += '\
                                    <div class="comment_answer">\
                                        <input class="openAnswerSomeone" type="hidden" value="0" />\
                                        <input class="toName" type="hidden" value="'+ answer.fullname + '"/>\
                                        <div class="com_name">\
                                            <h3>';

                        if (answer.type == "admin") {
                            html += '\
                                                <span>\
                                                    <img src="/assets/crown.png" width="14" height="14" title="Yönetici" />\
                                                </span>';
                        }

                        html +=
                            answer.fullname +
                            '</h3>\
                                        </div>\
                                        <div class="com_comment">';
                        if (answer.answerToName != null) {
                            html += '<p><span class="answerToName">' + answer.answerToName + ' </span>' + answer.text + '</p>';
                        } else {
                            html += '<p>' + answer.text + '</p>';
                        }

                        html += '\
                                        </div>\
                                        <div class="com_other">\
                                            <a>Yanıtla</a><span>'+ dateAnswer.getDate() + '.' + (dateAnswer.getMonth() < 10 ? '0' : '') + (dateAnswer.getMonth() + 1) + '.' + dateAnswer.getFullYear() + ' ' + dateAnswer.getHours() + ':' + (dateAnswer.getMinutes() < 10 ? '0' : '') + dateAnswer.getMinutes() + '</span>\
                                        </div>\
                                    </div>';

                    });

                    thisElement.parent().after(html);

                    if (result.answCount > 0) {
                        thisElement.html("Önceki yanıtları gör (" + result.answCount + ")");
                    } else {
                        thisElement.parent().remove();
                    }

                    moreLessControl();
                },
                error: function (errormessage) {
                    alert(errormessage.responseText);
                }
            });
        });
    }
}

function getAnswerSomeone() {
    if ($("#detail-content").length) {
        $("body").delegate("#detail-content .comment .com_other a", "click", function (e) {
            var commentId = $(this).parents(".comment").children(".CommentId").val();
            var toName = $(this).parents(".comment_answer").children(".toName").val();
            var toNameComment = $(this).parents(".comment").children(".toNameComment").val();
            var openAnswer = $(this).parent().siblings(".openAnswerSomeone").val();

            if (openAnswer == "0") {
                $(this).parent().siblings(".openAnswerSomeone").val("1");
                var html = '\
                        <div class="blog_comment_post">\
                             <form>\
                                <input class="commentId" type="hidden" value="'+ commentId + '" />';

                if (toName != null) {
                    html += '<input class="toName" type="hidden" value="' + toName + '" />';
                }
                else if (toNameComment != null) {
                    html += '<input class="toNameComment" type="hidden" value="' + toNameComment + '" />';
                }

                html += '\
                                <div class="form-group">\
                                    <label for="commentArea">Yorumu Yanıtlayın</label>\
                                    <textarea class="form-control commentArea" id="commentArea" rows="3" placeholder="Yorumu Yanıtlayın"></textarea>\
                                </div>\
                                <div class="form-row">\
                                    <div class="form-group col-md-6">\
                                        <label for="fullName">İsminiz</label>\
                                        <input type="text" class="form-control fullName" id="fullName" placeholder="İsminizi Giriniz">\
                                    </div>\
                                    <div class="form-group col-md-6">\
                                        <label for="email">Email Adresiniz</label>\
                                        <input type="email" class="form-control email" id="email" placeholder="Email Adresiniz">\
                                    </div>\
                                </div>\
                                <div class="form-row">\
                                    <div class="form-group col-6 col-sm-5 col-md-4" >\
                                        <div class="captchaArea">\
                                            <img class="captcha" src = "captcha-image-loading" />\
                                            <div class="captchaRefresh" onclick="captchaRefresh()" >\
                                                <img src="/assets/refresh.png" width="24" height="24" />\
                                            </div>\
                                        </div >\
                                    </div >\
                                    <div class="form-group col-6 col-sm-7 col-md-8">\
                                        <input id="CaptchaCode" name="CaptchaCode" type="text" class="captchaUserInput form-control" placeholder="Doğrulama Kodu" maxlength="5" />\
                                    </div>\
                                </div >\
                                <button type="button" class="btn btn-block btn-sm">Yorumu Gönder</button>\
                            </form>\
                        </div>';
                $(this).parent().after(html);
                var d = new Date();
                $(".captcha").attr("src", "/captcha-image-loading?" + d.getTime());
            } else {
                $(this).parent().siblings(".openAnswerSomeone").val("0");
                $(this).parent().siblings(".blog_comment_post").remove();
            }


        });
    }
}

function postComment() {
    if ($("#detail-content").length) {
        $("body").delegate("#detail-content .blog_comment_post button", "click", function (e) {
            var captchaUserInput = $(this).parent().find(".captchaUserInput").val();
            var captchaValidate = false;

            if (InputControl($(this))) {
                $.ajax({
                    url: "/Home/PostCaptchaUserInputCode",
                    data: {
                        CaptchaCode: captchaUserInput
                    },
                    type: "POST",
                    async: false,
                    success: function (result) {
                        if (result.validate) {
                            captchaValidate = true;
                        }
                    },
                    complete: function () {
                        var d = new Date();
                        $(".captcha").attr("src", "/captcha-image-loading?" + d.getTime());
                    }
                });

                if (captchaValidate == true) {
                    var _blogId = $(this).parents("#detail-content").find(".BlogId").val();
                    var _fullName = $(this).parent().find(".fullName").val();
                    var _email = $(this).parent().find(".email").val();
                    var _comment = $(this).parent().find(".commentArea").val();
                    var _commentId = null;
                    var _toName = null;
                    var thisObject = $(this);

                    if ($(this).parent().find(".commentId").val() != null) {
                        _commentId = $(this).parent().find(".commentId").val();
                    }

                    if ($(this).parent().find(".toName").val() != null) {
                        _toName = $(this).parent().find(".toName").val();
                    }

                    if ($(this).parent().find(".toNameComment").val() != null) {
                        _toName = $(this).parent().find(".toNameComment").val();
                    }

                    $.ajax({
                        url: "/Home/PostComment",
                        data: {
                            FullName: _fullName,
                            Email: _email,
                            Comment: _comment,
                            CommentId: _commentId,
                            ToName: _toName,
                            BlogId: _blogId
                        },
                        type: "POST",
                        success: function (result) {
                            if (result == true) {
                                var html = '\
                            <div class="alert alert-success" role = "alert" >\
                                Yorumunuz başarıyla gönderildi. Onaylandıktan sonra eklenecektir. Teşekkür ederiz.\
                            </div >';


                                thisObject.parent().after(html);
                                thisObject.parent().remove();
                            }
                            else {
                                var html = '\
                            <div class="alert alert-danger" role = "alert" >\
                                Yorumunuz gönderilirken bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.\
                            </div >';


                                thisObject.parent().after(html);
                                thisObject.parent().remove();
                            }
                        },
                        error: function (errormessage) {
                            alert(errormessage.responseText);
                        }
                    });
                }
                else {
                    var html = '\
                            <div class="alert alert-warning alert-dismissible fade show" role = "alert" >\
                                Doğrulama kodu yanlış girildi! Lütfen tekrar deneyiniz.\
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">\
                                    <span aria-hidden="true" >&times;</span >\
                                </button >\
                            </div >';
                    $(this).parent().prepend(html);

                }
            }


        });
    }
}

function postEmailRegistration(e) {
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
                                '+ result.message +'\
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">\
                                    <span aria-hidden="true" >&times;</span >\
                                </button >\
                            </div >';
                    thisBtn.parent().prepend(html);
                }
                else if (result.messageType == 2) {
                    var html = '\
                            <div class="alert alert-success alert-dismissible fade show" role = "alert" >\
                                '+ result.message +'\
                            </div >';
                    thisBtn.parent().prepend(html);
                    thisBtn.parent().find(".form-group").remove();
                    thisBtn.parent().find("p").remove();
                    thisBtn.remove();
                }
                else if (result.messageType == 3) {
                    var html = '\
                            <div class="alert alert-danger alert-dismissible fade show" role = "alert" >\
                                 '+ result.message +'\
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

function InputControl(thisBtn) {
    var captchaUserInput = thisBtn.parent().find(".captchaUserInput").val();
    var commentUserInput = thisBtn.parent().find(".commentArea").val();
    var nameUserInput = thisBtn.parent().find(".fullName").val();
    var emailUserInput = thisBtn.parent().find(".email").val();

    var captchaUserInputCount = captchaUserInput.length;
    var commentUserInputCount = commentUserInput.length;
    var nameUserInputCount = nameUserInput.length;
    var emailUserInputCount = emailUserInput.length;

    if (captchaUserInputCount == 0 || commentUserInputCount == 0 || nameUserInputCount == 0 || emailUserInputCount == 0) {
        var html = '\
                            <div class="alert alert-warning alert-dismissible fade show" role = "alert" >\
                                Lütfen tüm alanları doldurunuz!\
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">\
                                    <span aria-hidden="true" >&times;</span >\
                                </button >\
                            </div >';
        thisBtn.parent().prepend(html);
        return false;
    }

    if (commentUserInputCount < 15) {
        var html = '\
                            <div class="alert alert-warning alert-dismissible fade show" role = "alert" >\
                                Yorum alanı en az 15 karakter olmalıdır!\
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">\
                                    <span aria-hidden="true" >&times;</span >\
                                </button >\
                            </div >';
        thisBtn.parent().prepend(html);
        return false;
    }

    if (nameUserInputCount < 3) {
        var html = '\
                            <div class="alert alert-warning alert-dismissible fade show" role = "alert" >\
                                İsim alanı en az 3 karakter olmalıdır!\
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">\
                                    <span aria-hidden="true" >&times;</span >\
                                </button >\
                            </div >';
        thisBtn.parent().prepend(html);
        return false;
    }

    var nameControl = new RegExp(/^[a-zA-Z-çğıöşüÇĞİÖŞÜ]+$/);
    if (!nameControl.test(nameUserInput)) {
        var html = '\
                            <div class="alert alert-warning alert-dismissible fade show" role = "alert" >\
                                İsminiz harfler dışında herhangi bir karekter içeremez!\
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">\
                                    <span aria-hidden="true" >&times;</span >\
                                </button >\
                            </div >';
        thisBtn.parent().prepend(html);
        return false;
    }

    var emailControl = new RegExp(/^[^0-9][a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*[@][a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*[.][a-zA-Z]{2,4}$/i);
    if (!emailControl.test(emailUserInput)) {
        var html = '\
                            <div class="alert alert-warning alert-dismissible fade show" role = "alert" >\
                                Bu bir email adresi değil! Lütfen uygun formatta bir email adresi giriniz. (example@example.com)\
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">\
                                    <span aria-hidden="true" >&times;</span >\
                                </button >\
                            </div >';
        thisBtn.parent().prepend(html);
        return false;
    }

    if (captchaUserInputCount < 5) {
        var html = '\
                            <div class="alert alert-warning alert-dismissible fade show" role = "alert" >\
                                Doğrulama kodu 5 karakterli olmalıdır!\
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">\
                                    <span aria-hidden="true" >&times;</span >\
                                </button >\
                            </div >';
        thisBtn.parent().prepend(html);
        return false;
    }

    return true;
}

function moreLessControl() {
    var showComment = 200;

    $(".com_comment").each(function () {
        var comment = $(this).children("p").html();
        if (comment.length > showComment && $(this).find("span").hasClass("more") == false) {
            var showNow = comment.substr(0, showComment);
            var showAfter = comment.substr(showComment, comment.length - showComment)
            var html = '\
                        <p>'+ showNow + '\
                        <span class="treePoint">...</span>\
                        <span class="more">Devamı</span>\
                        <span class="showAfter">'+ showAfter + '</span>\
                        <span class="less">Küçült</span></p>';
            $(this).children().remove();
            $(this).append(html);

        }
    });

    $(".com_comment .more").click(function () {
        $(this).siblings(".showAfter").show();
        $(this).siblings(".less").show();
        $(this).siblings(".treePoint").hide();
        $(this).hide();
    });

    $(".com_comment .less").click(function () {
        $(this).siblings(".treePoint").show();
        $(this).siblings(".more").show();
        $(this).siblings(".showAfter").hide();
        $(this).hide();
    });

}