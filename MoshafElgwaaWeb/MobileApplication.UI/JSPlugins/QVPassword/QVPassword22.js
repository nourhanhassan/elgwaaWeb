/// <reference path="jquery-1.8.2.min.js" />


(function ($) {

    var check;
    $.fn.QVConfirmPassword = function (isNormalForm, options) {
        //
        var elem = $(this);
        var id = $(elem).attr('id');
        elem.hide();
        
        // my Arabic object
        var objAr = {
            name: "Arabic",
            tooShort: "قصير جدا",
            weak: "ضعيف",
            good: "جيد",
            strong: "قوى",
            password: " كلمة المرور الجديدة",
            confirm: "تأكيد كلمة المرور",
            fillpassword: "من فضلك ادخل كلمة المرور .",
            fillsamepassword: "نفس كلمة المرور الحإلية",
            fillconfirm: "من فضلك ادخل تأكيد كلمة المرور .",
            inCorrectPassword: "كلمة المرور غير مطابقة .",
            validation: "كلمة المرور يجب أن لا تقل عن 8 حروف وأرقام ورموز خاصة وتحتوي على الأقل رقم واحد"
        };

        // my English object
        var objEn = {
            name: "English",
            tooShort: "Too Short",
            weak: "Weak",
            good: "Good",
            strong: "Strong",
            password: "Password ",
            confirm: "Confirm Password ",
            fillpassword: "Please enter password.",
            fillsamepassword: "The same old password",
            fillconfirm: "Please re-enter password.",
            inCorrectPassword: "Password not matching.",
            validation: "at least 8 char ,contains spectial character , at least contains one number"
        };

        //Validation password RegEx between 4 and 8 char
        // var RegEx = /(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*[!#\$%&\?]).{4,8}$/;
        // var RegEx = /(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*[!#\$%&\?])^\D.{7}/;
        // var RegEx = /(?=^.{4,8}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$/;
        var RegEx = /^(?=.*\d).{8,}$/;

        // This is the easiest way to have default options.
        var settings = $.extend({
            // These are the defaults.
            language: objAr,
        }, options);


        if (settings.language === objAr.name || settings.language === "") {

            // set arabic language.
            settings.language = objAr;
            var direction = 'direction:rtl;';

        } else if (settings.language === objEn.name) {
            // set englsih language.
            settings.language = objEn;
            var direction = 'direction:ltr;';

        }
        //
        //HTML of textboxs and labels
        var passwrodHtml = '';

        //errorMsg
        var errorPassword = '<div class="text-danger error_' + id + '" style="display:none;">' + settings.language.fillpassword + '</div>';
        var errorsamePassword = '<div class="text-danger same_error_' + id + '" style="display:none;">' + settings.language.fillsamepassword + '</div>';
        var errorConfirm = '<div class="text-danger confirmpassword_error_' + id + '" style="display:none;">' + settings.language.fillconfirm + '</div>';
        var validatePassword = '<div class="text-danger validpassword_error_' + id + '" style="display:none;">' + settings.language.validation + '</div>';
        var inCorrectPassword = '<div class="text-danger inCorrectpassword_error_' + id + '" style="display:none;">' + settings.language.inCorrectPassword + '</div>';
        
        if (isNormalForm == true) { //HTML FOr Normal Form
            
            passwrodHtml = '<div class="form-group"  style="' + direction + '"><div class="row">' +
               '<label for="disabledinput" class="col-sm-3 control-label">' + settings.language.password + '</label><div class="col-sm-6">' +
               '<input type="password" class="form-control" id="password_' + id +
           '"  placeholder="' + settings.language.password +
           '">' +
               '</div><div id="result_' + id + '"></div></div></div>'
       +
       '<div class="form-group"  style="' + direction + '"><div class="row">' +
           '<label for="disabledinput" class="col-sm-3 control-label">' + settings.language.password + '</label><div class="col-sm-6">' +
           '<input type="password" class="form-control" id="confirmpassword_' + id + '" placeholder="' + settings.language.confirm +
           '">' +
           '</div></div></div>'
        }
        else { //HTML For PoPup

            passwrodHtml = '<div class="form-group"  style="' + direction + '"><div class="col-sm-12">' +
                '<div class="input-group">' +
                '<span class="input-group-addon"><i class="fa fa-lock"></i></span>' +
                '<input type="password" class="form-control" id="password_' + id +
            '"  placeholder="' + settings.language.password +
            '">' +
                '</div> ' + errorPassword + errorsamePassword + validatePassword +
            '<div id="result_' + id + '"></div></div></div>'
        +
        '<div class="form-group"  style="' + direction + '"><div class="col-sm-12">' +
            '<div class="input-group">' +
            '<span class="input-group-addon"><i class="fa fa-lock"></i></span>' +
            '<input type="password" class="form-control" id="confirmpassword_' + id + '" placeholder="' + settings.language.confirm +
            '">' +
            '</div>' + errorConfirm + inCorrectPassword + ' </div></div>';
        }
        // var passwrodHtml = '<div class="register register_' + id +
        //'" style="' + direction + '"><label for="password">' + settings.language.password +
        //'</label><input name="password" id="password_' + id +
        //'" type="password" placeholder="' + settings.language.password +
        //'"/><span id="result_' + id + '"></span><br/><label for="password">' + settings.language.confirm +
        //'</label><input name="password" id="confirmpassword_' + id + '" type="password" placeholder="' + settings.language.confirm +
        //'"/></div>';

        elem.after(passwrodHtml);

        /*
        assigning focusout event to confirm password field
            so if password confirm is correct remove any error message
           */

        //To check isvalid or not
        var IsValid;
        $('#confirmpassword_' + id + '').keyup(function () {

            IsValid = checkConfirm(id);

            console.log('confirm' + check);
            if (IsValid == false) {
                elem.attr('valid', false);
                console.log('confirm false');
            }
            else if (IsValid == true) {
                elem.attr('valid', true);
                elem.val($(this).val());
            }
        });

        /*
               assigning keyup event to password field
               so everytime user type code will execute
           */


        $('#password_' + id + '').keyup(function () {
            $('#result_' + id + '').html(checkStrength($('#password_' + id + '').val(), id));

            IsValid = checkConfirm(id);

            console.log('confirm' + check);
            if (IsValid == false) {
                elem.attr('valid', false);
                console.log('confirm false');
            }
            else if (IsValid == true) {
                elem.attr('valid', true);
                elem.val($(this).val());
            }
        });

        /*
       checkStrength is function which will do the 
       main password strength checking for us
   */
        function checkStrength(password, id) {
            //initial strength
            var strength = 0
            var password = $('#password_' + id + '').val();

            //if the password length is less than 6, return message.
            if (password.length < 6) {
                $('#result_' + id + '').removeClass();
                $('#result_' + id + '').addClass('short');
                return settings.language.tooShort;
            }

            //length is ok, lets continue.

            //if length is 8 characters or more, increase strength value
            if (password.length > 7) strength += 1

            //if password contains both lower and uppercase characters, increase strength value
            if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) strength += 1

            //if it has numbers and characters, increase strength value
            if (password.match(/([a-zA-Z])/) && password.match(/([0-9])/)) strength += 1

            //if it has one special character, increase strength value
            if (password.match(/([!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1

            //if it has two special characters, increase strength value
            if (password.match(/(.*[!,%,&,@,#,$,^,*,?,_,~].*[!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1

            //now we have calculated strength value, we can return messages

            //if value is less than 2
            if (strength < 2) {
                $('#result_' + id + '').removeClass()
                $('#result_' + id + '').addClass('weak')
                return settings.language.weak;
            }
            else if (strength == 2) {
                $('#result_' + id + '').removeClass()
                $('#result_' + id + '').addClass('good')
                return settings.language.good;
            }
            else {
                $('#result_' + id + '').removeClass()
                $('#result_' + id + '').addClass('strong')
                return settings.language.strong;
            }

        }
        /*
            checkConfirm is function which will check
             confirmarion password 
           */
        function checkConfirm(id) {

            // //
            //To hide error message
            $('.error_' + id + '').hide();
            var hasError = false;

            //password input value
            var passwordVal = $('#password_' + id + '').val();


            //confirm password input value
            var checkVal = $('#confirmpassword_' + id + '').val();

            //check values
            if (passwordVal == '') {
                //$('#password_' + id + '').parent().after('<div class="text-danger error_' + id + '">' + settings.language.fillpassword + '</div>');
                $('.error_' + id + '').css("display", "block");
                hasError = true;

            }
            if (checkVal == '') {
                $('.confirmpassword_error_' + id + '').css("display", "block");
                hasError = true;
            }

            //if (checkVal == '') {
            //    $('#confirmpassword_' + id + '').parent().after('<span class="text-danger confirmpassword_error_' + id + '">' + settings.language.fillconfirm + '</span>');
            //    hasError = true;
            //}
            if (!passwordVal.match(RegEx)) {
                var res = passwordVal.match(RegEx);
                $('.validpassword_error_' + id + '').css("display", "block");
                //  $('#password_' + id + '').parent().after('<span class="text-danger error_' + id + '">' + settings.language.validation + '</span>');
                $('#demo').html("dddd   " + res);
                hasError = true;
            }

            //else if ((checkVal == '')) {

            //    $('#confirmpassword_' + id + '').parent().after('<span class="text-danger confirmpassword_error_' + id + '">' + settings.language.fillconfirm + '</span>');
            //    hasError = true;

            //}
            if (passwordVal != checkVal) {
                $('.confirmpassword_error_' + id + '').css("display", "none");
                $('.inCorrectpassword_error_' + id + '').css("display", "block");
                //  $('#confirmpassword_' + id + '').parent().after('<span class="text-danger confirmpassword_error_' + id + '">' + settings.language.inCorrectPassword + '</span>');
                hasError = true;
            }

            else if (passwordVal == checkVal) {
                hasError = false;
            }

            if (hasError == true) { return false; }
            else if (hasError == false) { return true; }
        }

        function EmptyPassHandel(id) {
            IsValid = checkConfirm(id);

            console.log('confirm' + check);
            if (IsValid == false) {
                elem.attr('valid', false);
                console.log('confirm false');
            }
            else if (IsValid == true) {
                elem.attr('valid', true);
                elem.val($(this).val());
            }
        }
        return { EmptyPassHandel: EmptyPassHandel(id) }
    };

}(jQuery));