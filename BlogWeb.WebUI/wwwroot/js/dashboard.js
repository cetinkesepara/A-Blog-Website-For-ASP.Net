/** Sidebar */
function sbOpenClose(){

    if (window.matchMedia('(max-width: 767.98px)').matches){
        var sbWidth = $("#sidebar aside").css("width");
        if(sbWidth == "260px"){
            $("#sbclose").css({"display":"none"});
            $("#sbopen").css({"display":"block"});
            $(".menu-item").css({"width":"60px","display":"none"});
            $(".menu-item p").css({"display":"none"});
            $(".menu-item svg").css({"margin-left":"25%"});
            $(".menu-item .svg-bg").css({"border-top-right-radius":"4px", "border-bottom-right-radius":"4px", "background-color":"#adb5bd"});
            $(".sb-active .svg-bg").css({"border-top-right-radius":"4px", "border-bottom-right-radius":"4px", "background-color":"#3a6ea8"});
            $(".menu-item .svg-bg").hover(function(){$(this).css("background-color","#BEC5CB")},function(){$(this).css("background-color","#adb5bd")});
            $(".sb-active .svg-bg").hover(function(){$(this).css("background-color","#3a6ea8")});
            $("#sidebar aside").css({"width":"70px","background-color":"transparent"});

        }
        else {
            $("#sbopen").css({"display":"none"});
            $("#sbclose").css({"display":"block"});
            $(".menu-item").css({"width":"100%","display":"block"});
            $(".menu-item p").css({"display":"block"});
            $(".menu-item svg").css({"margin-left":"8%"});
            $(".menu-item .svg-bg").css({"border-top-right-radius":"0px", "border-bottom-right-radius":"0px", "background-color":"#3a6ea8"});
            $(".menu-item .svg-bg").hover(function(){$(this).css("background-color","#3a6ea8")});
            $("#sidebar aside").css({"width":"260px","background-color":"#e9ecef"});
        }
    }
    else {
        var sbWidth = $("#sidebar aside").css("width");
        if(sbWidth == "260px"){
            $("#sbclose").css({"display":"none"});
            $("#sbopen").css({"display":"block"});
            $(".menu-item").css({"width":"60px"});
            $(".menu-item p").css({"display":"none"});
            $(".menu-item svg").css({"margin-left":"25%"});
            $(".menu-item .svg-bg").css({"border-top-right-radius":"4px", "border-bottom-right-radius":"4px", "background-color":"#adb5bd"});
            $(".sb-active .svg-bg").css({"border-top-right-radius":"4px", "border-bottom-right-radius":"4px", "background-color":"#3a6ea8"});
            $(".menu-item .svg-bg").hover(function(){$(this).css("background-color","#BEC5CB")},function(){$(this).css("background-color","#adb5bd")});
            $(".sb-active .svg-bg").hover(function(){$(this).css("background-color","#3a6ea8")});
            $("#sidebar aside").css({"width":"70px"});
            $("#main section").css({"margin-left":"70px"});

            // Comments-Sidebar
            $("#main section .table-responsive .commentDialog .comDialog").css({ "left": "20%" });
        }
        else {
            $("#sbopen").css({"display":"none"});
            $("#sbclose").css({"display":"block"});
            $(".menu-item").css({"width":"100%"});
            $(".menu-item p").css({"display":"block"});
            $(".menu-item svg").css({"margin-left":"8%"});
            $(".menu-item .svg-bg").css({"border-top-right-radius":"0px", "border-bottom-right-radius":"0px", "background-color":"#3a6ea8"});
            $(".menu-item .svg-bg").hover(function(){$(this).css("background-color","#3a6ea8")});
            $("#sidebar aside").css({"width":"260px"});
            $("#main section").css({ "margin-left": "260px" });

            // Comments-Sidebar
            $("#main section .table-responsive .commentDialog .comDialog").css({ "left": "30%" });
        }
    }
}


/* Resim seçerken resim adýný yazdýrmak için */
$(document).ready(function () {
    var fileName
    $('input[type="file"]').change(function (e) {
        fileName = e.target.files[0].name;
        $('#resim_name').text(fileName);
        $('#resim_hata').hide();
    });
    $('#btn_kaydet').click(function (e) {
        if (fileName == null) {
            $('#resim_hata').show();
        }
    });
});