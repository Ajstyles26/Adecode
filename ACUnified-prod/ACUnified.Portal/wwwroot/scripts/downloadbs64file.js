
    function downloadBase64File(base64Data, filename) {
    // Convert base64 to raw binary data held in a string
    var byteString = atob(base64Data.split(',')[1]);

    // Separate out the mime component
    var mimeString = base64Data.split(',')[0].split(':')[1].split(';')[0];

    // Write the bytes of the string to a typed array
    var ia = new Uint8Array(byteString.length);
    for (var i = 0; i < byteString.length; i++) {
    ia[i] = byteString.charCodeAt(i);
}

    var blob = new Blob([ia], {type:mimeString});
    var url = window.URL.createObjectURL(blob);

    var link = document.createElement('a');
    link.href = url;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    setTimeout(function(){
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);
}, 100);
}

