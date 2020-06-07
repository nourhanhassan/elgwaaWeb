/// <reference path="../JS/jquery-1.7.2.min.js" />
(function ($) {
    debugger;
    $.fn.QVSqweezeImageUploader = function (options) {
        debugger
        var elem = $(this);
        var Uploader_Id = $(elem).attr('id') + "_QVUploader_";
        var settings = $.extend({

            type: 'png,jpg,jpeg,pdf,doc,docx,xls',
            size: '2',
            path: '/DataImages/Media/',
            width: '',
            height: '',
            lang: 'ar',
            displayImage: 'off',
            pixelsWidth: '',
            pixelsHeight: '',
            ConcatPrefix: '_C',
            value: elem.val(),
            callback: function () { },
            isCropImage: false,
            showZoomButtons: true,
            isResolutionImage: true,
        }, options);



        ///Messages
        extenionMessage = ' إمتداد الملف غير صحيح يجب أن يكون: ' + settings.type + '';
        sizeMessage = settings.size + 'Mb :' + 'خطأ... حجم الملف أكبر من المسموح به وهو '
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

        if (settings.isCropImage) {

            var Uploader_Html = ' <input  type="hidden" id="' + Uploader_Id + 'IsUploaded" />' +
                              '<input class="fileUpload" id="' + Uploader_Id + 'attach_file" name="" type="file" style="width: 225px;"> <input class="btnNonBrowseUpload" type="button" id="' + Uploader_Id + 'btnUpload" style="display: none" value="' + UploadTxt + '" />' +
                              '<br/><span class="spanUpload" id="' + Uploader_Id + 'upload_label" style="font-size: 10px; color: gray;"></span> <img class="loadedImage"  id="' + Uploader_Id + 'uploadedImage" src="" /> <input id="' + Uploader_Id + 'attached_file" type="hidden" />' +
                              '<img id="' + Uploader_Id + 'pop_loader" src="/JSPlugins/QVSqweezeImageUploader/images/AjaxLoader.gif" style="display: none;" /> <a id="' + Uploader_Id + 'ClearUpload" class="clearUpload">' + clear + '</a>' +
                              '<div id="' + Uploader_Id + '_CropImageContainer" class="cropImageContainer"> <div id="' + Uploader_Id + '_ImageBox" class="imageBox"><div class="thumbBox"></div></div><div class="CropImagebtnContainer"><input type="button" id="' + Uploader_Id + '_btn_CropImageUpload" value="' + UploadTxt + '" /><input type="button" class="btn_zoomIn" id="' + Uploader_Id + '_btnZoomIn" value="+" ><input type="button" class="btn_zoomOut" id="' + Uploader_Id + '_btnZoomOut" value="-" ></div></div>'



        }
        else {
            var Uploader_Html = '<input  type="hidden" id="' + Uploader_Id + 'IsUploaded" />' +
                               '<input class="fileUpload" id="' + Uploader_Id + 'attach_file" name="" type="file" style="width: 225px;"> <label class="lblUploadCheck" >' + UploadCheck + ' </label>  <input class="chbxUpload" type="checkbox" id="' + Uploader_Id + 'UploadonBrowse" checked="checked" /> <input class="btnNonBrowseUpload" type="button" id="' + Uploader_Id + 'btnUpload" style="display: none" value="' + UploadTxt + '" />' +
                               '<br/><span class="spanUpload" id="' + Uploader_Id + 'upload_label" style="font-size: 10px; color: gray;"></span> <img class="loadedImage"  id="' + Uploader_Id + 'uploadedImage" src="" /> <input id="' + Uploader_Id + 'attached_file" type="hidden" />' +
                               '<img id="' + Uploader_Id + 'pop_loader" src="/JSPlugins/QVSqweezeImageUploader/images/AjaxLoader.gif" style="display: none;" /> <a id="' + Uploader_Id + 'ClearUpload" class="clearUpload">' + clear + '</a>';
        }
        //hide the used textbox
        elem.css('display', 'none');

        elem.after(Uploader_Html);

        //If isCropImage flag is set
        if (settings.isCropImage) {
            var cropper;

            //Crop options
            var cropOptions =
            {
                thumbBox: '.thumbBox',
                spinner: '.spinner',
                imgSrc: 'avatar.png',
                width: settings.width,
                height: settings.height,
            }

            //Attach event on selecting a file
            $('#' + Uploader_Id + 'attach_file').on('change', function () {
                debugger
                var selectedFile = this.files[0];
                //If the type of the selectedFile is image, then display it for cropping
                if ((selectedFile.type.toLowerCase().indexOf('image') != -1)) {

                    //$(".CropImagebtnContainer").show(); //Show the crop buttons
                    $('#' + Uploader_Id + '_CropImageContainer').show();
                    //Check if the showZoomButtons flag is not set then hide the zoom buttons

                    if (!settings.showZoomButtons) {
                        $('#' + Uploader_Id + '_btnZoomIn').hide();
                        $('#' + Uploader_Id + '_btnZoomOut').hide();

                    }

                    var cropReader = new FileReader();
                    cropReader.onload = function (e) {
                        cropOptions.imgSrc = e.target.result;
                        cropper = $('#' + Uploader_Id + '_ImageBox').cropbox(cropOptions);

                    }

                    cropReader.readAsDataURL(this.files[0]);
                    this.files = [];
                }
                else { //Otherwise, call the upload function to show the validation message

                    Upload(this, settings.type, settings.size, settings.path, settings.width, settings.height, settings.lang, settings.displayImage, settings.callback,settings.isResolutionImage, settings.pixelsWidth, settings.pixelsHeight, settings.ConcatPrefix);
                }


            })


            //Attach events for zoom in and zoom out

            $('#' + Uploader_Id + '_btnZoomIn').on('click', function () {
                cropper.zoomIn();
            })
            $('#' + Uploader_Id + '_btnZoomOut').on('click', function () {
                cropper.zoomOut();
            })

            //Attach event on the upload button click
            $('#' + Uploader_Id + '_btn_CropImageUpload').on("click", function () {
                debugger;
                $('#' + Uploader_Id + '_btn_CropImageUpload').prop("disabled", true);
                var img = cropper.getDataURL();
                var fileName = $('#' + Uploader_Id + 'attach_file')[0].files[0].name;
                var croppedFile = dataURItoBlob(img, fileName);
                console.log(croppedFile);

                var attachElement = $('#' + Uploader_Id + "attach_file");
                Upload(attachElement, settings.type, settings.size, settings.path, settings.width, settings.height, settings.lang, settings.displayImage, function () {
                    $('#' + Uploader_Id + '_CropImageContainer').hide();
                    if (typeof (settings.callback) == "function") {
                        settings.callback();
                    }

                }, settings.isResolutionImage, settings.pixelsWidth, settings.pixelsHeight, settings.ConcatPrefix, croppedFile);

                //console.log(file);
                //console.log("sss");

            });


        }

        //in case the textbox have value
        debugger;
        if (settings.value != '') {


            $("#" + Uploader_Id + "uploadedImage").attr('src', settings.path + settings.value);
            $("#" + Uploader_Id + "upload_label").text(settings.value).css('color', '#BEC123');
            if (settings.displayImage != 'off') {
                if ($("#" + Uploader_Id + "uploadedImage")[0].Done == false) {
                    $("#" + Uploader_Id + "uploadedImage").attr('src', '/JSPlugins/QVSqweezeImageUploader/images/no_image_thumb.gif');
                }
                $("#" + Uploader_Id + "uploadedImage").show();
            }

        }


        $("#" + Uploader_Id + "UploadonBrowse").change(function (e) {
            e.preventDefault();
            if (!$(this).attr('checked'))
                $("#" + Uploader_Id + "btnUpload").css('display', 'block');
            else
                $("#" + Uploader_Id + "btnUpload").css('display', 'none');

        });

        //when select file to upload
        $("#" + Uploader_Id + "attach_file").on('change', function () {
            debugger
            var myfile = this;
            if (!$("#" + Uploader_Id + "UploadonBrowse").attr('checked')) {

                $("#" + Uploader_Id + "btnUpload").click(function () {

                    Upload(myfile, settings.type, settings.size, settings.path, settings.width, settings.height, settings.lang, settings.displayImage, settings.callback, settings.isResolutionImage, settings.pixelsWidth, settings.pixelsHeight, settings.ConcatPrefix);
                });
            } else if (!settings.isCropImage) {

                Upload(this, settings.type, settings.size, settings.path, settings.width, settings.height, settings.lang, settings.displayImage, settings.callback, settings.isResolutionImage, settings.pixelsWidth, settings.pixelsHeight, settings.ConcatPrefix);
            }

        });


        //clear value
        $("#" + Uploader_Id + "ClearUpload").bind('click', function (e) {

            e.preventDefault();
            elem.val('');
            $("#" + Uploader_Id + "upload_label").text('');
            $("#" + Uploader_Id + "pop_loader").hide();
            $('#' + Uploader_Id + '_btn_CropImageUpload').prop("disabled", false);
            // $("#" + Uploader_Id + "attached_file").val('');
            $("#" + Uploader_Id + "IsUploaded").val('');
            $("#" + Uploader_Id + "uploadedImage").attr('src', '');
            $("#" + Uploader_Id + "uploadedImage").hide();
            $("#" + Uploader_Id + "attach_file").val('');

        });


    };

    function dataURItoBlob(dataURI, fileName) {
        // convert base64/URLEncoded data component to raw binary data held in a string
        var byteString;
        if (dataURI.split(',')[0].indexOf('base64') >= 0)
            byteString = atob(dataURI.split(',')[1]);
        else
            byteString = unescape(dataURI.split(',')[1]);

        // separate out the mime component
        var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

        // write the bytes of the string to a typed array
        var ia = new Uint8Array(byteString.length);
        for (var i = 0; i < byteString.length; i++) {
            ia[i] = byteString.charCodeAt(i);
        }

        var blob = new Blob([ia], { type: mimeString });
        blob.name = fileName;

        return blob;



    }

    function Upload(e, type, size, path, width, height, lang, displayImage, callback,isResolutionImage,pixelsWidth,pixelsHeight,ConcatPrefix, croppedFile) {
        debugger
        var myTxtbox = $("#" + $(e).attr('id').split('_QVUploader_')[0])
        var Uploader_Id = $(e).attr('id').split('attach_file')[0];

        //incase that the file object is sent(cropped image Data URI was converted to blob object then sent)
        if (typeof (croppedFile) != "undefined") {

            var file = croppedFile;
        }

        else {
            var file = e.files[0];
        }
        // in case if press cancel
        if (typeof (file) == "undefined") {
            $("#" + Uploader_Id + "attach_file").val('');
            //$("#" + Uploader_Id + "attached_file").val('');
        }
        else {

            //get the first file
            //var file = e.files[0];

            //check if the file is image and the width and height sent as paramter and that the file is not cropped file
            if ((file.type.toLowerCase().indexOf('image') != -1) && (width != '' || height != '') && (typeof (croppedFile) == "undefined")) {

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
                            $('#' + Uploader_Id + '_btn_CropImageUpload').prop("disabled", false);

                        } else {

                            Confirm(e, type, size, path, width, height, lang, displayImage, file, callback,isResolutionImage, pixelsWidth, pixelsHeight, ConcatPrefix, croppedFile);
                        }

                    };
                });

                reader.readAsDataURL(file);
            } else {

                Confirm(e, type, size, path, width, height, lang, displayImage, file, callback, isResolutionImage, pixelsWidth, pixelsHeight, ConcatPrefix, croppedFile);
            }

        }
    }


    function Confirm(e, type, size, path, width, height, lang, displayImage, file, callback, isResolutionImage, pixelsWidth, pixelsHeight, ConcatPrefix, croppedFile) {

        var myTxtbox = $("#" + $(e).attr('id').split('_QVUploader_')[0])
        var Uploader_Id = $(e).attr('id').split('attach_file')[0];
        console.log(Uploader_Id);
        //check if the extention is valid
        if (type.indexOf(file.name.split('.').pop().toLowerCase()) != -1) {

            //check if the size is valid
            //1024 * 1024= 1048576
            if ((size * 1048576) > file.size) {

                $("#" + Uploader_Id + "attach_file").parent().attr({
                    'method': 'POST',
                    'enctype': 'multipart/form-data',
                    'target': '/JSPlugins/QVSqweezeImageUploader/UploadFile.ashx'
                });


                var formdata = false;
                if (window.FormData)
                    formdata = new FormData();

                //var file = e.files[0];
                if (formdata) {
                    formdata.append("attached_file", file, file.name);
                    formdata.append("folder", path);
                    formdata.append("isResolutionImage", isResolutionImage);
                    formdata.append("pixelsWidth", pixelsWidth);
                    formdata.append("pixelsHeight", pixelsHeight);
                    formdata.append("ConcatPrefix", ConcatPrefix);
                    $("#" + Uploader_Id + "pop_loader").show();
                    $('#' + Uploader_Id + '_btn_CropImageUpload').prop("disabled", true);
                    $('input:submit').attr('disabled', 'disabled');
                    $('input:submit').css("opacity", "0.3");
                    $('input:submit').css("cursor", "not-allowed");

                    $("#" + Uploader_Id + "IsUploaded").val("Progress");

                    $.ajax({
                        url: "/JSPlugins/QVSqweezeImageUploader/UploadFile.ashx",
                        type: "POST",
                        data: formdata,
                        processData: false,
                        contentType: false,
                        xhrFields:
                            {
                                onprogress: function (e) {

                                    if (e.lengthComputable) {

                                        //    $("#" + Uploader_Id + "upload_label").html('(Uploading .. ' + (e.loaded / e.total * 100) + '%)').color('orange');
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
                                        $("#" + Uploader_Id + "uploadedImage").attr('src', image.src);
                                        $("#" + Uploader_Id + "uploadedImage").show();
                                    };
                                });

                                reader.readAsDataURL(file);

                            }
                            var res = eval(res)[0];

                            if (res.status == 'done') {
                                //set my textbox by the filename
                                myTxtbox.val(res.filename);
                                debugger;
                                $(myTxtbox).trigger("change").trigger("input");
                                try{
                                    $(myTxtbox).valid();
                                } catch (s) {
                                }
                                $("#" + Uploader_Id + "upload_label").html(successMessage).css('color', 'green');
                                debugger
                                callback();
                                FormData.file = $("#" + Uploader_Id + "attach_file").val().trim();
                            }
                            else
                                $("#" + Uploader_Id + "upload_label").html(errorMessage).css('color', 'red');

                            $("#" + Uploader_Id + "pop_loader").hide();
                            $('#' + Uploader_Id + '_btn_CropImageUpload').prop("disabled", false);
                            $('input:submit').removeAttr('disabled');
                            $('input:submit').css("opacity", "1");
                            $('input:submit').css("cursor", "pointer");
                        },
                        error: function () {
                            $("#" + Uploader_Id + "pop_loader").hide();
                            $('#' + Uploader_Id + '_btn_CropImageUpload').prop("disabled", false);
                            $("#" + Uploader_Id + "upload_label").html(errorMessage).css('color', 'red');
                        }
                    });
                }
            }
            else {
                $("#" + Uploader_Id + "upload_label").html(sizeMessage).css('color', 'red');
                $('#' + Uploader_Id + '_btn_CropImageUpload').prop("disabled", false);

            }
        }
        else {
            $("#" + Uploader_Id + "upload_label").html(extenionMessage).css('color', 'red');
            $('#' + Uploader_Id + '_btn_CropImageUpload').prop("disabled", false);
        }
    }
}(jQuery));












//"use strict";
(function (factory) {
    if (typeof define === 'function' && define.amd) {
        define(['jquery'], factory);
    } else {
        factory(jQuery);
    }
}(function ($) {
    var cropbox = function (options, el) {
        var el = el || $(options.imageBox),
            obj =
            {
                state: {},
                ratio: 1,
                options: options,
                imageBox: el,
                thumbBox: el.find(options.thumbBox),
                spinner: el.find(options.spinner),
                image: new Image(),
                getDataURL: function () {
                    var width = this.thumbBox.width(),
                        height = this.thumbBox.height(),
                        canvas = document.createElement("canvas"),
                        dim = el.css('background-position').split(' '),
                        size = el.css('background-size').split(' '),
                        dx = parseInt(dim[0]) - el.width() / 2 + width / 2,
                        dy = parseInt(dim[1]) - el.height() / 2 + height / 2,
                        dw = parseInt(size[0]),
                        dh = parseInt(size[1]),
                        sh = parseInt(this.image.height),
                        sw = parseInt(this.image.width);

                    canvas.width = width;
                    canvas.height = height;
                    var context = canvas.getContext("2d");
                    context.drawImage(this.image, 0, 0, sw, sh, dx, dy, dw, dh);
                    var imageData = canvas.toDataURL('image/png');
                    return imageData;
                },
                getBlob: function () {
                    var imageData = this.getDataURL();
                    var b64 = imageData.replace('data:image/png;base64,', '');
                    var binary = atob(b64);
                    var array = [];
                    for (var i = 0; i < binary.length; i++) {
                        array.push(binary.charCodeAt(i));
                    }
                    return new Blob([new Uint8Array(array)], { type: 'image/png' });
                },
                zoomIn: function () {
                    this.ratio *= 1.1;
                    setBackground();
                },
                zoomOut: function () {
                    this.ratio *= 0.9;
                    setBackground();
                }
            },
            setBackground = function () {
                var w = parseInt(obj.image.width) * obj.ratio;
                var h = parseInt(obj.image.height) * obj.ratio;

                var pw = (el.width() - w) / 2;
                var ph = (el.height() - h) / 2;

                el.css({
                    'background-image': 'url(' + obj.image.src + ')',
                    'background-size': w + 'px ' + h + 'px',
                    'background-position': pw + 'px ' + ph + 'px',
                    'background-repeat': 'no-repeat'
                });
            },
            imgMouseDown = function (e) {
                e.stopImmediatePropagation();

                obj.state.dragable = true;
                obj.state.mouseX = e.clientX;
                obj.state.mouseY = e.clientY;
            },
            imgMouseMove = function (e) {
                e.stopImmediatePropagation();

                if (obj.state.dragable) {
                    var x = e.clientX - obj.state.mouseX;
                    var y = e.clientY - obj.state.mouseY;

                    var bg = el.css('background-position').split(' ');

                    var bgX = x + parseInt(bg[0]);
                    var bgY = y + parseInt(bg[1]);

                    el.css('background-position', bgX + 'px ' + bgY + 'px');

                    obj.state.mouseX = e.clientX;
                    obj.state.mouseY = e.clientY;
                }
            },
            imgMouseUp = function (e) {
                e.stopImmediatePropagation();
                obj.state.dragable = false;
            },
            zoomImage = function (e) {
                e.originalEvent.wheelDelta > 0 || e.originalEvent.detail < 0 ? obj.ratio *= 1.1 : obj.ratio *= 0.9;
                setBackground();
            }


        obj.imageBox.css("width", 1.1 * options.width + "px");
        obj.imageBox.css("height", 1.1 * options.height + "px");

        obj.thumbBox.css("width", options.width + "px");
        obj.thumbBox.css("height", options.height + "px");
        obj.imageBox.show();

        obj.thumbBox.css("marginLeft", -1 * (obj.thumbBox.width() / 2) + "px")
        obj.thumbBox.css("marginTop", -1 * (obj.thumbBox.height() / 2) + "px")


        obj.spinner.show();
        obj.image.onload = function () {
            obj.spinner.hide();
            setBackground();

            el.bind('mousedown', imgMouseDown);
            el.bind('mousemove', imgMouseMove);
            $(window).bind('mouseup', imgMouseUp);
            el.bind('mousewheel DOMMouseScroll', zoomImage);
        };
        obj.image.src = options.imgSrc;
        el.on('remove', function () { $(window).unbind('mouseup', imgMouseUp) });

        return obj;
    };

    jQuery.fn.cropbox = function (options) {
        return new cropbox(options, this);
    };
}));