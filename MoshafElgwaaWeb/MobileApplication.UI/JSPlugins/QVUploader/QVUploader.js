/// <reference path="../JS/jquery-1.7.2.min.js" />


(function ($) {

    $.fn.QVUploader = function (options) {

        var elem = $(this);
        //console.log(elem);
        var Uploader_Id = $(elem).attr('id') + "_QVUploader_";
        elem.addClass("uploader_input");
        var settings = $.extend({

            type: 'png,PNG,jpg,jpeg,pdf,doc,docx,xls',
            size: '1',
            path: '/ControlPanel/DataImages/File/',
            width: '',
            height: '',
            lang: 'ar',
            displayImage: 'on',
            value: elem.val(),
            previewMdeiaType: ''
        }, options);




        ///Messages
        extenionMessage = ' إمتداد الملف غير صحيح يسمح بالامتدادات الاتية ' + settings.type;
        sizeMessage = ' خطأ... حجم الملف أكبر من المسموح به وهو ' + settings.size + 'Mb'
        errorMessage = 'خطأ... حاول مرة أخرى';
        successMessage = 'لقد تم تحميل الملف بنجاح';
        dimentionMessage = 'خطأ... احداثيات الصورة لابد ان تكون : عرض ' + settings.width + ' * طول ' + settings.height + ' من فضلك ';
        UploadTxt = "تحميل";
        UploadCheck = "تحميل تلقائي:";
        clear = 'مسح';

        //incase if lang = en
        if (settings.lang != "ar") {
            extenionMessage = "This file type is not supported, the supported is: " + settings.type;
            sizeMessage = 'Error... The size of file is more than Permitted which is:' + settings.size + 'Mb';
            errorMessage = 'Error... Please try again';
            successMessage = 'The file uploaded successfully';
            dimentionMessage = 'Error... The dimensions of this image must be ' + settings.width + ' width * ' + settings.height + ' height';
            UploadTxt = "Upload";
            UploadCheck = "Upload on browse:";
            clear = 'Clear';
        }

        //the html of Uploader 
        var conteainerDivID = "divIdentify_" + Uploader_Id;
        var HTML_uploadedContentPreview = '';
        switch (settings.previewMdeiaType) {
            case 'Image':
                HTML_uploadedContentPreview = '<img class="loadedImage"  id="' + Uploader_Id + 'uploadedImage" src="" />'
                break;
            case 'Vedio':
                HTML_uploadedContentPreview = '<img class="loadedImage"  id="' + Uploader_Id + 'uploadedImage" src="" />'
                break;
            case 'Audio':
                HTML_uploadedContentPreview = '<audio controls class="loadedImage"  id="' + Uploader_Id + 'uploadedImage" src=""> </audio>';
                break;
            default:
                HTML_uploadedContentPreview = '<img class="loadedImage"  id="' + Uploader_Id + 'uploadedImage" src="" />'
                break;

        }

        var Uploader_Html = '<div id="' + conteainerDivID + '"> <input  type="hidden" id="' + Uploader_Id + 'IsUploaded" />' +
                           '<input class="fileUpload" id="' + Uploader_Id + 'attach_file" name="" type="file" style="width: 200px;"><label class="lblUploadCheck" >' + UploadCheck + ' </label>  <input class="chbxUpload" type="checkbox" id="' + Uploader_Id + 'UploadonBrowse" checked="checked" /> <input class="btnNonBrowseUpload" type="button" id="' + Uploader_Id + 'btnUpload" style="display: none" value="' + UploadTxt + '" />' +
                           '<span class="spanUpload" id="' + Uploader_Id + 'upload_label" style="font-size: 10px; color: gray;"></span>' +
                           HTML_uploadedContentPreview +
                            ' <input id="' + Uploader_Id + 'applicant_file" type="hidden" />' +
                           '<img id="' + Uploader_Id + 'pop_loader" src="/JsPlugins/QVUploader/images/AjaxLoader.gif" style="display: none;" /><a id="' + Uploader_Id + 'ClearUpload" class="clearUpload">' + clear + '</a></div> ';
        elem.change(function () {
            $("#" + conteainerDivID).remove();
            elem.QVUploader(options);
        })
        //hide the used textbox
        elem.css('display', 'none');

        elem.after(Uploader_Html);


        //in case the textbox have value


        if (settings.value != '') {

            $("#" + Uploader_Id + "uploadedImage").attr('src', settings.value);
            debugger

            if (settings.displayImage != 'off') {
                if ($("#" + Uploader_Id + "uploadedImage")[0].Done == false) {
                    $("#" + Uploader_Id + "uploadedImage").attr('src', '/JSPlugins/QVUploader/images/no_image_thumb.gif');
                }
                $("#" + Uploader_Id + "uploadedImage").show();
            }

        }

        $("#" + Uploader_Id + "UploadonBrowse").change(function (e) {
            
            e.preventDefault();

            //console.log(this.checked);
            //if (!$(this).attr('checked'))
            if (!this.checked)
                //$("#" + Uploader_Id + "btnUpload").css('display', 'block');
                $("#" + Uploader_Id + "btnUpload").show();

            else
                //$("#" + Uploader_Id + "btnUpload").css('display', 'none');
                $("#" + Uploader_Id + "btnUpload").hide();

        });
        //$("#" + Uploader_Id + "attach_file").click(function () {
        //    
        //    var myfile = this;
        //    console.log($("#" + Uploader_Id + "UploadonBrowse").is(':checked'));
        //    //if (!$("#" + Uploader_Id + "UploadonBrowse").attr('checked')) {
        //    if (!$("#" + Uploader_Id + "UploadonBrowse").attr('checked')) {
        //        
        //        $("#" + Uploader_Id + "btnUpload").click(function () {
        //            
        //            Upload(myfile, settings.type, settings.size, settings.path, settings.width, settings.height, settings.lang, settings.displayImage);
        //        });
        //    } else {
        //        
        //        Upload(this, settings.type, settings.size, settings.path, settings.width, settings.height, settings.lang, settings.displayImage);
        //    }
        //})
        //when select file to upload
        $("#" + Uploader_Id + "attach_file").on('change', function () {
            
            var myfile = this;
            //if (!$("#" + Uploader_Id + "UploadonBrowse").attr('checked')) {
            if (!$("#" + Uploader_Id + "UploadonBrowse").is(':checked')) {
                
                $("#" + Uploader_Id + "btnUpload").click(function () {
                    
                    Upload(myfile, settings.type, settings.size, settings.path, settings.width, settings.height, settings.lang, settings.displayImage);
                });
            } else {
                
                Upload(this, settings.type, settings.size, settings.path, settings.width, settings.height, settings.lang, settings.displayImage);
            }
        });

        //clear value
        $("#" + Uploader_Id + "ClearUpload").bind('click', function (e) {

            e.preventDefault();
            elem.val('');

            $("#" + Uploader_Id + "upload_label").text('');
            $("#" + Uploader_Id + "pop_loader").hide();
            $("#" + Uploader_Id + "applicant_file").val('');
            $("#" + Uploader_Id + "IsUploaded").val('');
            $("#" + Uploader_Id + "uploadedImage").attr('src', '');
            $("#" + Uploader_Id + "uploadedImage").hide();
            $("#" + Uploader_Id + "attach_file").val('');

            //hide clear button
            $(this).hide();
        });

    };

    function Upload(e, type, size, path, width, height, lang, displayImage) {


        var myTxtbox = $("#" + $(e).attr('id').split('_QVUploader_')[0])
        console.log(myTxtbox)
        var Uploader_Id = $(e).attr('id').split('attach_file')[0];



        // in case if press cancel 
        if (e.files.length == 0) {

            $("#" + Uploader_Id + "applicant_file").val('');
        }
        else {

            //get the first file
            var file = e.files[0];

            //check if the file is image and the width and height sent as paramter
            if ((file.type.indexOf('image') != -1) && (width != '' || height != '')) {

                //create FileReader to get the file url
                var reader = new FileReader();
                reader.onload = (function (theFile) {
                    //create Image and set the src by the reader url
                    var image = new Image();
                    image.src = theFile.target.result;

                    image.onload = function () {
                        // in case if the width and height of the image not equal the sent in paramter, we dont upload and show the message below
                        if (this.width != width || this.height != height) {

                            $("#" + Uploader_Id + "upload_label").html(dimentionMessage).css('color', 'red');

                        } else {

                            Confirm(e, type, size, path, width, height, lang, displayImage, file);
                        }

                    };
                });

                reader.readAsDataURL(file);
            } else {

                Confirm(e, type, size, path, width, height, lang, displayImage, file);
            }

        }
    }

    function Confirm(e, type, size, path, width, height, lang, displayImage, file) {

        var myTxtbox = $("#" + $(e).attr('id').split('_QVUploader_')[0])
        var Uploader_Id = $(e).attr('id').split('attach_file')[0];

        //check if the extention is valid
        if (type.indexOf(file.name.split('.').pop()) != -1) {

            //check if the size is valid
            //1024 * 1024= 1048576
            if ((size * 1048576) >= file.size) {

                $("#" + Uploader_Id + "attach_file").parent().attr({
                    'method': 'POST',
                    'enctype': 'multipart/form-data',
                    'target': '/JSPlugins/QVUploader/UploadFile.ashx'
                });


                var formdata = false;
                if (window.FormData)
                    formdata = new FormData();

                var file = e.files[0];
                if (formdata) {
                    formdata.append("applicant_file", file);
                    formdata.append("folder", path);
                    $("#" + Uploader_Id + "pop_loader").show();
                    $("#" + Uploader_Id + "IsUploaded").val("Progress");
                    var ajaxRequest = $.ajax({
                        url: "/JSPlugins/QVUploader/UploadFile.ashx",
                        type: "POST",
                        data: formdata,
                        processData: false,
                        contentType: false,
                        xhrFields:
                            {
                                onprogress: function (e) {

                                    if (e.lengthComputable) {

                                        $("#" + Uploader_Id + "upload_label").html('(Uploading .. ' + (e.loaded / e.total * 100) + '%)');
                                    }
                                }
                            },
                        success: function (res) {

                            $("#" + Uploader_Id + "IsUploaded").val("Done");
                            if (file.type.indexOf('image') != -1 && displayImage != "off") {

                                var reader = new FileReader();
                                reader.onload = (function (theFile) {
                                    //create Image and set the src by the reader url
                                    var image = new Image();

                                    image.src = theFile.target.result;

                                    image.onload = function () {
                                        // alert($("#" + Uploader_Id + "uploadedImage").get(0));
                                        $("#" + Uploader_Id + "uploadedImage").attr('src', image.src);
                                        $("#" + Uploader_Id + "uploadedImage").show();
                                    };
                                    myTxtbox.trigger("input");

                                });

                                reader.readAsDataURL(file);

                            }
                            //else if (file.type.indexOf('audio') != -1 && displayImage != "off") {

                            //    $("#" + Uploader_Id + "uploadedImage").attr('src', path + eval(JSON.stringify(eval(res)))[1].filename);
                            //    alert($("#" + Uploader_Id + "uploadedImage").attr('src'));
                            //}
                            //var res = eval(res)[0];

                            var res = eval(JSON.stringify(eval(res)))[0];

                            if (res.status == 'done') {
                                //set my textbox by the filename
                                myTxtbox.val(res.filename)
                                $("#" + Uploader_Id + "upload_label").html(successMessage).css('color', 'green');

                                FormData.file = $("#" + Uploader_Id + "applicant_file").val().trim();
                            }
                            else
                                $("#" + Uploader_Id + "upload_label").html(errorMessage).css('color', 'red');

                            $("#" + Uploader_Id + "pop_loader").hide();
                            // alert("in");
                            myTxtbox.trigger("input");
                            $("#" + Uploader_Id + "ClearUpload").show();
                        },
                        error: function (xhr, text_status, error_thrown) {
                            
                            if (text_status != "abort") {
                                $("#" + Uploader_Id + "pop_loader").hide();
                                $("#" + Uploader_Id + "upload_label").html(errorMessage).css('color', 'red');
                            }


                        }
                    });
                    
                    try {
                        if (typeof ajaxRequests !== 'undefined') {
                            ajaxRequests.push(ajaxRequest);
                        }
                    } catch (e) {

                    }



                }
            }
            else {
                $("#" + Uploader_Id + "upload_label").html(sizeMessage).css('color', 'red');

            }
        }
        else {
            $("#" + Uploader_Id + "upload_label").html(extenionMessage).css('color', 'red');
        }
    }

    function survey(selector, callback) {
        var input = $(selector);
        var oldvalue = input.val();
        setInterval(function () {
            if (input.val() != oldvalue) {
                oldvalue = input.val();
                callback();
            }
        }, 100);
    };

}(jQuery));