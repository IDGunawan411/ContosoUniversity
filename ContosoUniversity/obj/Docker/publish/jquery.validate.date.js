$(function () {
    $.validator.methods.date = function (value, Enrollment) {
        if ($.browser.webkit) {
            var d = new Date();
            return this.optional(Enrollment) || !/Invalid|NaN/.test(new Date(d.toLocaleDateString(value)));
        }
        else {
            return this.optional(Enrollment) || !/Invalid|NaN/.test(new Date(value));
        }
    };
});
