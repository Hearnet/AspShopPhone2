function Validator1(formSelector){
    var formRules= {};
    var _this = this;
    function getParent(element, selector) {
        while (element.parentElement) {
            if (element.parentElement.matches(selector)) {
                return element.parentElement;
            }
            element = element.parentElement;
        }
    }

    var validatorRules={
        required: function (value){
            return value ? undefined : 'Vui lòng nhập trường này';
        },
        email: function (value){
            var regex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;

            return regex.test(value) ? undefined : 'Trường này phải là email';
        },
        min: function (min){
            return function (value){
            return value.length >= min ? undefined : `Vui lòng nhập tối thiểu ${min} kí tự`;               
            }
        },
        max: function (max){
            return function (value){
            return value.length <= max ? undefined : `Vui lòng nhập tối đa ${max} kí tự`;               
            }
        }
    }

    var formElement=document.querySelector(formSelector);

    if (formSelector){
        var inputs= formElement.querySelectorAll('[name][rules]');
        for (var input of inputs){   
            
            var rules =input.getAttribute('rules').split('|');
            for (var rule of rules){
                var isHasKey = rule.includes(':');
                var ruleFunc = undefined;

                if (isHasKey){
                    var ruleInfo = rule.split(':');
                    rule= ruleInfo[0];
                    ruleFunc = validatorRules[rule](ruleInfo[1]);
                } else {
                    ruleFunc=validatorRules[rule];
                }
            
                if (Array.isArray(formRules[input.name])){
                    formRules[input.name].push(ruleFunc);
                } else {
                    formRules[input.name]=[ruleFunc];
                }
            
            }
            

            // lắng nghe sự kiện validate
            input.onblur = handleValidate;
            input.oninput = handleClearErr;
        }
        
    }
    function handleValidate(e){
        var rules= formRules[e.target.name];
        var errMessage;
        for (rule of rules){
            errMessage=rule(e.target.value);
            if(errMessage) break;
        }
        if (errMessage){
            var frmGroup= getParent(e.target, '.form-group');
            frmGroup.classList.add('invalid');
            if (frmGroup){
                var frmMessage= frmGroup.querySelector('.form-message');

                if (frmMessage){
                    frmMessage.innerText= errMessage;
                }
            }
        } 
        return !errMessage;
    }
    function handleClearErr(e){
        var frmGroup= getParent(e.target, '.form-group');
        if (frmGroup.classList.contains('invalid')){
            frmGroup.classList.remove('invalid');

        }
        var frmMessage= frmGroup.querySelector('.form-message');

        if (frmMessage){
            frmMessage.innerText= "";
        }
    }


    //khi submit
    formElement.onsubmit= function (e){
        e.preventDefault();

      //  return; 


        var isFormValid = true;
        var inputs= formElement.querySelectorAll('[name][rules]');
        for (var input of inputs){   
            
            if (!handleValidate({target: input}))
                isFormValid=false;
        }

        if (isFormValid){
            if (typeof _this.onSubmit === 'function'){
                var enableInputs = formElement.querySelectorAll('[name]');
                    var formValues = Array.from(enableInputs).reduce(function (values, input) {
                        
                        switch(input.type) {
                            case 'radio':
                                values[input.name] = formElement.querySelector('input[name="' + input.name + '"]:checked').value;
                                break;
                            case 'checkbox':
                                if (!input.matches(':checked')) {
                                    values[input.name] = '';
                                    return values;
                                }
                                if (!Array.isArray(values[input.name])) {
                                    values[input.name] = [];
                                }
                                values[input.name].push(input.value);
                                break;
                            case 'file':
                                values[input.name] = input.files;
                                break;
                            default:
                                values[input.name] = input.value;
                        }

                        return values;
                    }, {});
                    _this.onSubmit(formValues);
            } else
                formElement.submit();
        }
    }
}