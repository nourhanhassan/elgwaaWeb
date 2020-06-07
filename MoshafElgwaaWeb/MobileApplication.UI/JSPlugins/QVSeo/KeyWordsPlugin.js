var counter = 0;
var metaCounter = 0;
var KeyWordsArray = new Array();
var MetaKeyWordsArray = new Array();
var totalKeyWords = new Array();
var testKeyWords = new Array();
var pluginFlag = false;
var targetKeyWordValid = false;
var pageTitleValid = false;
var pageDescriptionValid = false;
var metaKeyWordValid = false;
var articleId;

(function ($) {
    // sort of extension method to merge 2 arrays and remove duplicated items
    Array.prototype.unique = function () {
        var a = this.concat();
        for (var i = 0; i < a.length; ++i) {
            for (var j = i + 1; j < a.length; ++j) {
                if (a[i] === a[j])
                    a.splice(j--, 1);
            }
        }

        return a;
    };
    ////////////
    //Attatch the new method		   
    jQuery.fn.extend({

        //Plugin Name
        SEOPlugin: function (options, iArticleId) { // options is a json object where the first and last elements are array of strings whereas the other are just a string
            
            var pluginHtml = '<div class="searchPnl" style="direction: rtl;"><span id="generalErrorMsg" style="color: red; font-size: 16px; font-weight: bold;">تأكد من عدم ترك خانات فارغة</span><div class="clearfix"><div><div style="direction: rtl;"><label class="form-lbl">الكلمات المستهدفة:</label><input id="txtTargetKeyWord" type="text" style="width: 500px;"/><input type="button" value="إضافة" class="grren-btn" id="btnTargeteAdd" style="margin-right: 10px;height: 30px;"/><span class="KeyWordErrorMsg" id="chainError" style="color: red; font-size: 16px; font-weight: bold;"></span></div></br><div id="divKeyWords"><div dir="rtl"><ul class="sortable" id="ulKeyWords"><li id="LiLoading" style="text-align: center; background-color: White;"></li></ul></div></div></br><div dir="rtl"><label class="form-lbl">عنوان الصفحة:</label><input id="txtPageTitle" type="text" style="width: 500px;margin-right: 27px;" /><span class="TitleErrorMsg" id="Span1" style="color: red; font-size: 16px; font-weight: bold;"></span></div></br><div dir="rtl"><label class="form-lbl" style="position: relative;top: -40px;">وصف الصفحة:</label><textarea id="txtPageDescription" rows="6" cols="50" style="width: 493px;margin-right: 30px;"></textarea><span class="DescriptionErrorMsg" id="Span2" style="color: red; font-size: 16px; font-weight: bold;"></span></div></br><div dir="rtl"><label class="form-lbl">الكلمات المفتاحية:</label><input id="txtMetaKeyWords" type="text" style="width: 500px;margin-right: 8px;"/><input type="button" value="إضافة" class="grren-btn" id="btnMetaAdd" style="margin-right: 10px;height: 30px;"/><span class="MetaKeyWordErrorMsg" id="Span3" style="color: red; font-size: 16px; font-weight: bold;"></span></div></br><div id="divMetaKeyWords"><div dir="rtl"><ul class="exclude" id="ulMetaKeyWords"><li id="LiMetaKeyWords" style="text-align: center; background-color: White;"></li></ul></div></div></div></div></div>';
            //return this.each(function () {
            $(this).append(pluginHtml);
            //Here is where you insert your normal code
            $(function () {
                 articleId= iArticleId;
                if (articleId == null)
                    articleId = 0;
                $("#generalErrorMsg").hide();
                if (options == null) { //insert case
                    $("#txtPageTitle").attr("disabled", "disabled");
                    $("#txtPageDescription").attr("disabled", "disabled");
                    $("#txtMetaKeyWords").attr("disabled", "disabled");
                    $('#ulKeyWords').children().remove();
                    $('#ulKeyWords').append('<li id="emptyLi"><a href="javascript:void(0)">لا يوجد كلمات مستهدفة</a></li>');
                    $('#ulMetaKeyWords').children().remove();
                    $('#ulMetaKeyWords').append('<li id="emptyLi"><a href="javascript:void(0)">لا يوجد كلمات مفتاحية</a></li>');
                }
                else { // update case
                    $('#ulKeyWords').children().remove();
                    $('#ulMetaKeyWords').children().remove();
                    var liCounter = 0;
                    for (var i = liCounter; i < options.TargetKeyWord.length; i++) { // for loop for the target keyword
                        $('#ulKeyWords').append('<li class="liChain" draggable="true" id="li_' + i + '"><a href="javascript:void(0)" >' + options.TargetKeyWord[i] + '</a><a href="#" class="deleteBtn">X</a></li>');
                        $("#ulMetaKeyWords").append('<li class="liChain" id="metali_' + i + '"><a href="javascript:void(0)" >' + options.TargetKeyWord[i] + '</a></li>');
                        liCounter++;
                    }
                    $("#txtPageTitle").val(options.PageTitle);
                    $("#txtPageDescription").val(options.PageDescription);
                    for (var j = liCounter; j < options.MetaKeyWords.length; j++) { // for loop for the metakeyword
                        $("#ulMetaKeyWords").append('<li class="liChain" id="meta_' + j + '"><a href="javascript:void(0)" >' + options.MetaKeyWords[j] + '</a><a href="javascript:void(0)" class="metaDeleteBtn">X</a></li>');
                    }
                    KeyWordsArray = options.TargetKeyWord;
                    MetaKeyWordsArray = options.MetaKeyWords;
                    //if (MetaKeyWordsArray.length > 0) {
                    //    totalKeyWords = KeyWordsArray.concat(MetaKeyWordsArray);
                    //} else {
                    //    totalKeyWords = KeyWordsArray;
                    //}
                    targetKeyWordValid = true;
                    pageTitleValid = true;
                    pageDescriptionValid = true;
                    metaKeyWordValid = true;
                }

            });
            //////////////////////////////////
            
            // To add  target keyword to the list
            $("#btnTargeteAdd").click(function () {
                var flag = true;
                if ($("#txtTargetKeyWord").val() != "") { //in case of enter key is pressed

                    var strTargetKeyWord = $("#txtTargetKeyWord").val();
                    if ($("#ulKeyWords li").size() < 3 && /^[a-zA-Z0-9\u0600-\u06ff,./\s_-]{1,70}$/.test(strTargetKeyWord)) {
                        if ($("#ulKeyWords li").children().first().text() == "لا يوجد كلمات مستهدفة") {
                            $("#txtPageTitle").attr("disabled", "disabled");
                            $("#txtPageDescription").attr("disabled", "disabled");
                            $("#txtMetaKeyWords").attr("disabled", "disabled");
                            $('#ulKeyWords').children().remove();
                            $('#ulMetaKeyWords').children().remove();
                            targetKeyWordValid = false;
                            counter = 0;
                        }
                        counter++;
                        if ($("#ulMetaKeyWords li").size() == 5) {
                            flag = false;
                            var index = $("#ulMetaKeyWords li")[counter - 2];
                            $("#ulMetaKeyWords li").last().remove();
                            $(index).after("<li class='liChain' id='metali_'" + counter + "'><a href='javascript:void(0)' >" + $('#txtTargetKeyWord').val() + "</a></li>");
                            targetKeyWordValid = true;
                        }
                        $("#ulKeyWords").append('<li class="liChain" draggable="true" id="li_' + counter + '"><a href="javascript:void(0)" >' + $("#txtTargetKeyWord").val() + '</a><a href="javascript:void(0)" class="deleteBtn">X</a></li>');
                        if (flag) {
                            $("#ulMetaKeyWords").append('<li class="liChain" id="metali_' + counter + '"><a href="javascript:void(0)" >' + $("#txtTargetKeyWord").val() + '</a></li>');
                            metaKeyWordValid = true;
                        }
                        $("#txtTargetKeyWord").val('');
                        $("#txtPageTitle").removeAttr("disabled");
                        $("#txtPageDescription").removeAttr("disabled");
                        $("#txtMetaKeyWords").removeAttr("disabled");
                        $(".KeyWordErrorMsg").text(' ');
                        KeyWordsArray.push(strTargetKeyWord);
                        targetKeyWordValid = true;
                    } else {
                        $(".KeyWordErrorMsg").text('الكلمة المستهدفة يجب أن لا تحتوي على رموز خاصة و أن لا تزيد عن 70 حرف و أن لا تزيد عدد الكلمات عن 3 كلمات');
                        targetKeyWordValid = false;
                    }
                }
            });
            //To add meta keyword to the list below
            $("#btnMetaAdd").click(function() {
                if ($("#txtMetaKeyWords").val() != "") { // in case of enter key is pressed
                    var strMetaKeyWord = $("#txtMetaKeyWords").val();
                    if ($("#ulMetaKeyWords li").size() < 5 && /^[a-zA-Z0-9\u0600-\u06ff,./\s_-]{1,70}$/.test(strMetaKeyWord)) {
                        if ($("#ulMetaKeyWords li").children().first().text() == "لا يوجد كلمات مفتاحية") {
                            $('#ulMetaKeyWords').children().remove();
                            counter = 0;
                            metaKeyWordValid = false;
                        }
                        metaCounter++;
                        $("#ulMetaKeyWords").append('<li class="liChain" id="meta_' + metaCounter + '"><a href="javascript:void(0)" >' + $("#txtMetaKeyWords").val() + '</a><a href="javascript:void(0)" class="metaDeleteBtn">x</a></li>');
                        $("#txtMetaKeyWords").val('');
                        $("#txtPageTitle").removeAttr("disabled");
                        $("#txtPageDescription").removeAttr("disabled");
                        $(".MetaKeyWordErrorMsg").text(' ');
                        metaKeyWordValid = true;
                        MetaKeyWordsArray.push(strMetaKeyWord);
                        totalKeyWords = KeyWordsArray.concat(MetaKeyWordsArray).unique();
                        testKeyWords = totalKeyWords;
                    } else {
                        $(".MetaKeyWordErrorMsg").text('الكلمة المستهدفة يجب أن لا تحتوي على رموز خاصة و أن لا تزيد عن 70 حرف و أن لا تزيد عدد الكلمات عن 5 كلمات');
                        metaKeyWordValid = false;
                    }
                }
            });
            //For the change in the text of the title textbox
            $("#txtPageTitle").on('change', function () {
                var strPageTitle = $("#txtPageTitle").val();
                if (/^[a-zA-Z0-9\u0600-\u06ff,./\s_-]{1,70}$/.test(strPageTitle) && strPageTitle.indexOf($("#ulKeyWords li").first().children().first().text()) >= 0) {
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "/ControlPanel/Content/CheckSeoTitleDuplication",//?id=" + articleId + "&strTitle=" + strPageTitle,
                        data: "{ 'id':'"+articleId+"','strTitle':'"+strPageTitle+"'}",
                        contentType: "application/json; charset=utf-8",
                        //dataType: "json",
                        success: function (result) {
                            
                            console.log(result);
                            if (result == "True") {
                                //alert("Good");
                                $(".TitleErrorMsg").text(' ');
                                pageTitleValid = true;
                            } else {
                                //alert("False");
                                console.log(result);
                                $(".TitleErrorMsg").text('هذا العنوان موجود بالفعل');
                            }
                        },
                        error: function (xhr, status, error) {
                            console.log(xhr.responseText);
                            //alert("Not Good");
                            pageTitleValid = false;
                        }
                    });
                } else {
                    $(".TitleErrorMsg").text('عنوان الصفحة يجب أن لا يزيد عن 70 حرف و يجب أن يحتوي على الكلمة المستهدفة الأولى');
                }
            });
            //For the change in the text of the title textbox
            $("#txtPageDescription").on('change', function () {
                var strPageDescription = $("#txtPageDescription").val();

                if (/^[a-zA-Z0-9\u0600-\u06ff,./\s_-]{70,150}$/.test(strPageDescription) ){//&& target) {
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "/ControlPanel/Content/CheckSeoDescriptionDuplication",//?id=" + articleId + "&strDescription=" + strPageDescription,
                        data: "{ 'id':'" + articleId + "','strDescription':'" + strPageDescription + "'}",
                        contentType: "application/json; charset=utf-8",
                        //dataType: "json",
                        success: function (result) {
                            
                            if (result == "True") {
                                $(".DescriptionErrorMsg").text(' ');
                                pageDescriptionValid = true;
                            }
                        },
                        error: function () {
                            //alert("Not Good");
                            pageDescriptionValid = false;
                            $(".DescriptionErrorMsg").text('هذا الوصف موجود بالفعل');
                        }
                    });
                } else {
                    $(".DescriptionErrorMsg").text(' وصف الصفحة يجب أن لا يزيد عن 150 حرف و أن لا يقل عن 70 حرف');
                }
            });
            /////////////////////////////////////
            $('#ulKeyWords li').on('mouseover', function () {
                $(this).css('cursor', 'move');
            });

            $('#ulKeyWords li a').on('mouseover', function () {
                $(this).css('cursor', 'move');
            });

            $('#ulMetaKeyWords li').on('mouseover', function () {
                $(this).css('cursor', 'move');
            });
            
            $('.deleteBtn').on('mouseover', function () {
                $(this).css('cursor', 'pointer');
            });

            //To delete the item from the list
            $(document).on('click', '.deleteBtn', function () {
                
                var id = $(this).parent().attr('id').split('li_')[1];
                var index = KeyWordsArray.indexOf($(this).parent().eq(0).text().toLowerCase().split('x')[0]);
                var indexMeta = MetaKeyWordsArray.indexOf($(this).parent().eq(0).text().toLowerCase().split('x')[0]);
                //remove the li item
                $(this).parent().remove();

                console.log(index);
                $("#ulMetaKeyWords li").attr('id', 'metali_' + id).eq(id - 1).remove();
                if (index > -1) {
                    KeyWordsArray.splice(index, 1);
                    //MetaKeyWordsArray.splice(index, 1);
                    totalKeyWords = KeyWordsArray.concat(MetaKeyWordsArray).unique();
                    testKeyWords = totalKeyWords;
                }
                if (indexMeta > -1) {
                    MetaKeyWordsArray.splice(indexMeta, 1);
                    totalKeyWords = KeyWordsArray.concat(MetaKeyWordsArray).unique();
                    testKeyWords = totalKeyWords;
                }
                
                counter = counter - 1;
                $("#ulMetaKeyWords").children().remove();
                var keyWordsCounter = 0;
                for (var j = 0; j < KeyWordsArray.length; j++) {
                    $("#ulMetaKeyWords").append('<li class="liChain" draggable="true" id="metali_' + j + '"><a href="javascript:void(0)">' + KeyWordsArray[j] + '</a></li>');
                    keyWordsCounter++;
                }

                for (var i = keyWordsCounter; i < testKeyWords.length; i++) {
                    $("#ulMetaKeyWords").append('<li class="liChain" id="metali_' + i + '"><a href="javascript:void(0)" >' + testKeyWords[i] + '</a><a href="javascript:void(0)" class="metaDeleteBtn">X</a></li>');
                }
                //check if the ul is empty
                if ($("#ulKeyWords li").size() == 0) {
                    // if the ul is empty it append the empty tepmlate to the ul
                    $("#txtPageTitle").attr("disabled", "disabled");
                    $("#txtPageDescription").attr("disabled", "disabled");
                    $('#ulKeyWords').append('<li id="emptyLi"><a href="javascript:void(0)">لا يوجد كلمات مستهدفة</a></li>');
                    $("#ulMetaKeyWords").children().remove();
                    $('#ulMetaKeyWords').append('<li id="emptymetaLi"><a href="javascript:void(0)">لا يوجد كلمات مستهدفة</a></li>');
                    $(".KeyWordErrorMsg").text(' ');
                }
                if ($("#ulMetaKeyWords li").size() == 0) {
                    // if the ul is empty it append the empty tepmlate to the ul
                    $('#ulMetaKeyWords').append('<li id="emptymetaLi"><a href="javascript:void(0)">لا يوجد كلمات مستهدفة</a></li>');
                    $(".MetaKeyWordErrorMsg").text(' ');
                }
            });
            //////////////////////////////

            $(document).on('click', ".metaDeleteBtn", function () {
                
                var id = $(this).parent().attr('id').split('meta_')[1];
                var index = MetaKeyWordsArray.indexOf($(this).parent().eq(0).text().toLowerCase().split('x')[0]);
                //console.log(id);
                //console.log($("#ulMetaKeyWords li").attr('id', 'metali_' + id)[0]);

                //remove the li item
                $(this).parent().remove();
                if (index > -1) {
                    MetaKeyWordsArray.splice(index, 1);
                    totalKeyWords = KeyWordsArray.concat(MetaKeyWordsArray).unique();
                    testKeyWords = totalKeyWords;
                }
                if (index == -1) {
                    MetaKeyWordsArray = KeyWordsArray;
                }
                if ($("#ulMetaKeyWords li").size() == 0) {
                    // if the ul is empty it append the empty tepmlate to the ul
                    $('#ulMetaKeyWords').append('<li id="emptymetaLi"><a href="javascript:void(0)">لا يوجد كلمات مستهدفة</a></li>');
                    $(".MetaKeyWordErrorMsg").text(' ');
                }
            });
            return {
                JsonData: function () {
                    var isValid = ValidationChecking();
                    if (isValid) {
                        var returnValue = CreateJsonData();
                        return returnValue;
                    }
                },
                ValidationChecking: function () {
                    if (targetKeyWordValid && metaKeyWordValid && pageTitleValid && pageDescriptionValid) {
                        pluginFlag = true;
                        $("#generalErrorMsg").hide();
                        return pluginFlag;
                    } else {
                        $("#generalErrorMsg").show();
                        pluginFlag = false;
                        return pluginFlag;
                    }
                }
            };
        }
        

    });

})(jQuery);

var CreateJsonData = function () {
    
    var seoJsonData = new Object();
    seoJsonData.TargetKeyWord = KeyWordsArray;
    seoJsonData.PageTitle = $("#txtPageTitle").val();
    seoJsonData.PageDescription = $("#txtPageDescription").val();
    seoJsonData.MetaKeyWords = totalKeyWords;
    return seoJsonData;
};

var ValidationChecking = function () {
    if (targetKeyWordValid && metaKeyWordValid && pageTitleValid && pageDescriptionValid) {
        pluginFlag = true;
        $("#generalErrorMsg").hide();
        return pluginFlag;
    } else {
        $("#generalErrorMsg").show();
        pluginFlag = false;
    }
};
