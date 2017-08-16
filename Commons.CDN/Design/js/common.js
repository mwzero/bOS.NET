function showMessage(msgType, header, body, details) {
    var divMessage = $("#message");
    if (msgType == "error")
        divMessage.addClass("alert-error");
    else if (msgType == "info") {
        divMessage.addClass("alert-success");
        $("#SheetContentPlaceHolder_ValidationSummary").hide();
    }

    divMessage.find("h4").text(header);
    var p = divMessage.find("#bodyMessage");
    //p.text(body);
    p.append(body);
    p.append(details);
    
    $("#message").show();
}