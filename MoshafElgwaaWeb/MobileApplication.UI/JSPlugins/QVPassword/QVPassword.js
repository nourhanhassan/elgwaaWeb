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
            password: " كلمة المرور ",
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

        //Validation password RegEx 8 numbers 
        // var RegEx = /^(?=.*\d).{8,}$/;

        //Validation password RegEx 8 char at least one upper , at least one lower , at least one special char , at least one number 
        // var RegEx = /(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!#\$%&\?]).{8,}$/;

        //Validation password RegEx 8 char at least one char , at least one special char , at least one number 
        var RegEx = /(?=.*[a-zA-Z])(?=.*\d)(?=.*[!@#\$%^&~*_\?/]).{8,}$/;

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
        var errorPassword = '<div class="errorMsg error_' + id + '" style="display:none;">' + settings.language.fillpassword + '</div>';
        var errorsamePassword = '<div class="errorMsg same_error_' + id + '" style="display:none;">' + settings.language.fillsamepassword + '</div>';
        var errorConfirm = '<div class="errorMsg confirmpassword_error_' + id + '" style="display:none;">' + settings.language.fillconfirm + '</div>';
        var validatePassword = '<div class="errorMsg validpassword_error_' + id + '" style="display:none;">' + settings.language.validation + '</div>';
        var inCorrectPassword = '<div class="errorMsg inCorrectpassword_error_' + id + '" style="display:none;">' + settings.language.inCorrectPassword + '</div>';

        //forNormalForm
        var errorPassword2 = '<span class="errorMsg error_' + id + '" style="display:none;">' + settings.language.fillpassword + '</span>';

        if (isNormalForm == true) { //HTML FOr Normal Form
            //
            passwrodHtml = '<div class="form-group"  style="' + direction + '"><div class="row">' +
               '<label for="disabledinput" class="col-sm-3 control-label">' + settings.language.password + '</label><span class="field-validation-indicator">*</span><div class="col-sm-6">' +
               '<input type="password" name="passwordValues" class="form-control" id="password_' + id +
           '"  placeholder="' + settings.language.password +
           '">' + errorPassword2 + validatePassword + '<div id="result_' + id + '"></div></div></div>' +
                '</div>'
       +
       '<div class="form-group"  style="' + direction + '"><div class="row">' +
           '<label for="disabledinput" class="col-sm-3 control-label">' + settings.language.confirm + '</label><span class="field-validation-indicator">*</span><div class="col-sm-6">' +
           '<input type="password" name="passwordValues" class="form-control" id="confirmpassword_' + id + '" placeholder="' + settings.language.confirm +
           '">' + errorConfirm + inCorrectPassword +
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
        //OLD
        // var passwrodHtml = '<div class="register register_' + id +
        //'" style="' + direction + '"><label for="password">' + settings.language.password +
        //'</label><input name="password" id="password_' + id +
        //'" type="password" placeholder="' + settings.language.password +
        //'"/><span id="result_' + id + '"></span><br/><label for="password">' + settings.language.confirm +
        //'</label><input name="password" id="confirmpassword_' + id + '" type="password" placeholder="' + settings.language.confirm +
        //'"/></div>';

        elem.after(passwrodHtml);

        var passwordElem = $('#password_' + id + '');
        var confirmpasswordElem = $('#confirmpassword_' + id + '');
        /*
        assigning focusout event to confirm password field
            so if password confirm is correct remove any error message
           */

        //To check isvalid or not
        var IsValid;
        $('#confirmpassword_' + id + '').keyup(function () {
            debugger
            IsValid = checkConfirm(id);

            //console.log('confirm' + check);
            if (IsValid == false) {

                // passwordElem.attr('valid', false);
                // confirmpasswordElem.prop('valid', false);
                passwordElem.prop('valid', false);
                console.log('confirm false');
            }
            else if (IsValid == true) {
                //  passwordElem.attr('valid', true);
                //confirmpasswordElem.prop('valid', true);
                passwordElem.prop('valid', true);
                debugger
                elem.val($(this).val());
                elem.trigger("input");
            }
        });

        /*
               assigning keyup event to password field
               so everytime user type code will execute
           */


        $('#password_' + id + '').keyup(function () {
            // debugger 
            $('#result_' + id + '').html(checkStrength($('#password_' + id + '').val(), id));

            IsValid = checkConfirm(id);
            console.log('confirm' + check);
            if (IsValid == false) {
                //  elem.attr('valid', false);
                passwordElem.prop('valid', false);

                console.log('confirm false');
            }
            else if (IsValid == true) {
                // elem.attr('valid', true);
                passwordElem.prop('valid', true);

                elem.val($(this).val());
                elem.trigger("input");
            }
        });

        /*
       checkStrength is function which will do the 
       main password strength checking for us
   */

        function checkStrength(password, id) {
            //initial strength
            var strength = 0
            // debugger 
            var password = $('#password_' + id + '').val();

            //if the password length is less than 6, return message.
            if (password.length < 6) {
                $('#result_' + id + '').removeClass();
                $('#result_' + id + '').addClass('short');
                return settings.language.tooShort;
            }

            //length is ok, lets continue.

            //if length is 8 characters or more, increase strength value
            // if (password.length > 7) strength += 1

            //if password contains both lower and uppercase characters, increase strength value
            if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) {

                // debugger
                strength += 1
            }

            //if it has numbers and characters, increase strength value
            if (password.match(/([a-zA-Z])/) && password.match(/([0-9])/)) {
                //  debugger
                strength += 1
            }

            //if it has one special character, increase strength value
            if (password.match(/([!,@,#,$,%,^,&,*,_,/,?,~])/)) strength += 1

            //if it has two special characters, increase strength value
            if (password.match(/(.*[!,@,#,$,%,^,&,*,_,/,?,~].*[ !,@,#,$,%,^,&,*,_,/,?,~])/)) strength += 1

            //now we have calculated strength value, we can return messages

            //if value is less than 2

            if (strength < 1) {
                //  debugger 
                $('#result_' + id + '').removeClass()
                $('#result_' + id + '').addClass('weak')

                return settings.language.weak;
            }
            else if (strength == 1) {
                // debugger 
                $('#result_' + id + '').removeClass()
                $('#result_' + id + '').addClass('good')

                return settings.language.good;
            }
            else {
                //debugger 
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

            //To hide error message
            $('.error_' + id + '').hide();
            var hasError = true;
            var confError = true;
            //password input value
            var passwordVal = $('#password_' + id + '').val();


            //confirm password input value
            var checkVal = $('#confirmpassword_' + id + '').val();

            //check values
            if (passwordVal == '') {
                //  debugger 
                $('.validpassword_error_' + id + '').css("display", "none");
                //$('#result_' + id + '').css("display", "none");
                $('.error_' + id + '').css("display", "block");
                strength = 0;
                hasError = false;

            }
            if (checkVal == '') {
                //  debugger
                $('.inCorrectpassword_error_' + id + '').css("display", "none");
                $('.confirmpassword_error_' + id + '').css("display", "block");
                //hasError = false;
                confError = true;

            }
            if (!passwordVal.match(RegEx)) {
                //  debugger
                if (passwordVal != '') {

                    var res = passwordVal.match(RegEx);
                    $('.same_error_' + id + '').css("display", "none");
                    $('#result_' + id + '').css("display", "block");
                    $('.validpassword_error_' + id + '').css("display", "block");
                    hasError = false;
                } else {
                    $('#result_' + id + '').css("display", "none");
                }

            }
            if (passwordVal.match(RegEx)) {
                //  debugger
                var res = passwordVal.match(RegEx);
                $('.validpassword_error_' + id + '').css("display", "none");
                hasError = true;

            }

            if (passwordVal != checkVal) {
                //  debugger
                $('.confirmpassword_error_' + id + '').css("display", "none");
                if (checkVal == '') {

                    $('.inCorrectpassword_error_' + id + '').css("display", "none");
                    $('.confirmpassword_error_' + id + '').css("display", "block");
                    //hasError = false;
                    confError = false;

                } else {
                    $('.inCorrectpassword_error_' + id + '').css("display", "block");
                    // hasError = false;
                    confError = false;
                }
            }

            if (passwordVal == checkVal) {
                //  debugger
                if (passwordVal == '') {
                    $('.error_' + id + '').css("display", "block");
                    hasError = false;

                }
                else if (checkVal == '') {
                    //    debugger
                    $('.inCorrectpassword_error_' + id + '').css("display", "none");
                    $('.confirmpassword_error_' + id + '').css("display", "block");
                    // hasError = false;
                    confError = false;

                } else {
                    //    debugger
                    //  $('.validpassword_error_' + id + '').css("display", "none");
                    $('.confirmpassword_error_' + id + '').css("display", "none");
                    $('.inCorrectpassword_error_' + id + '').css("display", "none");
                    // hasError = true;
                    confError = true;
                }
            }
            //the password strength should be medium or strong , if its weak so it'llnot be valid

            var res = (hasError && confError) == true ? true : false;
            return res;
            //return hasError;

            //  var rError = (hasError) == true ? false : true;
            // alert(rError);
            // return hasError;
            //if (hasError == true)  return false; 
            //else if (hasError == false)   return true; 
        }

        function EmptyPassHandel(id) {
            IsValid = checkConfirm(id);
            //alert(IsValid);
            //console.log('confirm' + check);
            if (IsValid == false) {
                //  elem.attr('valid', false);
                passwordElem.prop('valid', false);

                console.log('confirm false');
            }
            else if (IsValid == true) {
                // elem.attr('valid', true);
                passwordElem.prop('valid', false);

                // elem.val($(this).val());
            }
        }
        return { EmptyPassHandel: EmptyPassHandel(id) }
    };

}(jQuery));