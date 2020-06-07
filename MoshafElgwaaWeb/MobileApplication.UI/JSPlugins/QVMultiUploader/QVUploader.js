/// <reference path="../JS/jquery-1.7.2.min.js" />
(function ($) {

    $.fn.QVUploader = function (options) {

        var elem = $(this);
        var Uploader_Id = $(elem).attr('id') + "_QVUploader_";
        var settings = $.extend({

            type: 'gif,png,jpg,jpeg,pdf,doc,docx,xls',
            size: '2',
            path: '/DataImages/File/',
            width: '',
            height: '',
            lang: 'ar',
            displayImage: 'off',
            value: elem.val(),
            
        }, options);



        ///Messages
        extenionMessage = 'إمتداد الملف غير صحيح';
        sizeMessage = '  خطأ... حجم الملف أكبر من المسموح به وهو ' + settings.size + 'Mb' //settings.size + 
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

        //<span class="btn btn-success fileinput-button">
        //                                <i class="fa fa-plus"></i>
        //                                <span>إضافة ملفات...</span>
        //                                <input type="file" name="files[]" multiple="">
        //                            </span>

        //the html of Uploader 
        var Uploader_Html = '<form> <input  type="hidden" id="' + Uploader_Id + 'IsUploaded" />' +
                           '<input class="fileUpload uploadnew" id="' + Uploader_Id + 'attach_file" multiple="multiple" name="" type="file" style="display:none;"> ' +
                           '<span class="spanUpload" id="' + Uploader_Id + 'upload_label" style="font-size: 10px; color: gray;"></span> <img class="loadedImage"  id="' + Uploader_Id + 'uploadedImage" src="" /> <input id="' + Uploader_Id + 'applicant_file" type="hidden" />' +
                           '<img id="' + Uploader_Id + 'pop_loader" src="/JSPlugins/QVUploader/images/AjaxLoader.gif" style="display: none;"/></form>';

        //hide the used textbox
        elem.css('display', 'none');

        elem.after(Uploader_Html);


        //in case the textbox have value

        if (settings.value != '') {

            $("#" + Uploader_Id + "uploadedImage").attr('src', settings.path + settings.value);

            if (settings.displayImage != 'off') {
                if ($("#" + Uploader_Id + "uploadedImage")[0].Done == false) {
                    $("#" + Uploader_Id + "uploadedImage").attr('src', '/JSPlugins/QVUploader/images/no_image_thumb.gif');
                }
                $("#" + Uploader_Id + "uploadedImage").show();
            }

        }
        
            debugger
        $("#" + Uploader_Id + "UploadonBrowse").change(function (e) {
            e.preventDefault();
            if (!$(this).attr('checked'))
                $("#" + Uploader_Id + "btnUpload").css('display', 'block');
            else
                $("#" + Uploader_Id + "btnUpload").css('display', 'none');

        });
       
        //when select file to upload
        $("#" + Uploader_Id + "attach_file").live('change', function () {

            var myfile = this;
             Upload(this, settings.type, settings.size, settings.path, settings.width, settings.height, settings.lang, settings.displayImage);
           
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

        });


    };


    function Upload(e, type, size, path, width, height, lang, displayImage) {

        var myTxtbox = $("#" + $(e).attr('id').split('_QVUploader_')[0])
        var Uploader_Id = $(e).attr('id').split('attach_file')[0];

       // console.log(e.files);
        // in case if press cancel 
        if (e.files.length == 0) {

            $("#" + Uploader_Id + "applicant_file").val('');
        }
        else {

            for (var i = 0; i < e.files.length; i++) {

                var file = e.files[i];


                //check if the file is image and the width and height sent as paramter
                if ((file.type.toLowerCase().indexOf('image') != -1) && (width != '' || height != '')) {

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

            //get the first file
           
        }
    }


    function Confirm(e, type, size, path, width, height, lang, displayImage, file) {
        console.log(file);
        var myTxtbox = $("#" + $(e).attr('id').split('_QVUploader_')[0])
        var Uploader_Id = $(e).attr('id').split('attach_file')[0];
        //check if the extention is valid
        console.log(type);

        if (type.indexOf(file.name.split('.').pop().toLowerCase()) != -1) {

            //check if the size is valid
            //1024 * 1024= 1048576
            if ((size * 1048576) > file.size) {

                $("#" + Uploader_Id + "attach_file").parent().attr({
                    'method': 'POST',
                    'enctype': 'multipart/form-data',
                    'target': '/JSPlugins/QVMultiUploader/UploadFile.ashx'
                });


                var formdata = false;
                if (window.FormData)
                    formdata = new FormData();

                
                if (formdata) {
                    formdata.append("applicant_file", file);
                    formdata.append("folder", path);
                    $("#" + Uploader_Id + "pop_loader").show();
                    $('input:submit').attr('disabled', 'disabled');
                    $('input:submit').css("opacity", "0.3");
                    $('input:submit').css("cursor", "not-allowed");

                    $("#" + Uploader_Id + "IsUploaded").val("Progress");
                                  
                    $.ajax({
                        url: "/JSPlugins/QVMultiUploader/UploadFile.ashx",
                        type: "POST",
                        data: formdata,
                        processData: false,
                        contentType: false,
                        xhrFields:
                            {
                                onprogress: function (e) {

                                    if (e.lengthComputable) {

                                        $("#" + Uploader_Id + "upload_label").html('(Uploading .. ' + (e.loaded / e.total * 100) + '%)').color('orange');
                                    }
                                }
                            },
                        success: function (res) {
                            $("#" + Uploader_Id + "IsUploaded").val("Done");
                            
                            var res = eval(res)[0];

                            if (res.status == 'done') {
                                //set my textbox by the filename
                                myTxtbox.val(myTxtbox.val() + res.filename + ',');
                                myTxtbox.trigger("change");
                                $("#" + Uploader_Id + "upload_label").html(successMessage).css('color', 'green');

                               
                                var appendedElement = '<div class="form-group attachmentRecord">';
                                appendedElement+='<div class="col-sm-8 fileName ">'+file.name+'</div>';
                                appendedElement += '<div class="col-sm-2 downloadAttachment"><a href="/JSPlugins/QVMultiUploader/DownloadFile.ashx?filepath=' + path + res.filename + '&originalFileName=' + file.name + '">تحميل</a></div><div class="col-sm-2 deleteAttachment"> <a href="#">حذف</a></div>';

                                appendedElement += ' <input type="hidden" name="' + Uploader_Id + 'attachment_OriginalName" value="' + file.name + '" />';
                                appendedElement += '<input type="hidden" name="' + Uploader_Id + 'attachment_FileName" value="' + res.filename + '" />';
                                appendedElement += '<input type="hidden" name="' + Uploader_Id + 'attachment_ID" value="0" />  '
                                appendedElement += '</div>';

                                myTxtbox.parents(".QVMultiFileUploader").find(".uploadedFilesDiv").append(appendedElement);

                                //Change the status of the "show attachments" button
                                checkShowFilesStatus(myTxtbox.parents(".QVMultiFileUploader").first());

                                FormData.file = $("#" + Uploader_Id + "applicant_file").val().trim();
                                $(myTxtbox).trigger("change");
                            }
                            else
                                $("#" + Uploader_Id + "upload_label").html(errorMessage).css('color', 'red');

                            $("#" + Uploader_Id + "pop_loader").hide();
                            $('input:submit').removeAttr('disabled');
                            $('input:submit').css("opacity", "1");
                            $('input:submit').css("cursor", "pointer");
                        },
                        error: function () {
                            $("#" + Uploader_Id + "pop_loader").hide();
                            $("#" + Uploader_Id + "upload_label").html(errorMessage).css('color', 'red');
                        }
                    });
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
}(jQuery));
