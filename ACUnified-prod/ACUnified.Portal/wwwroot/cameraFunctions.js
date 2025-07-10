// wwwroot/cameraFunctions.js


window.isNavigatorMediaDevicesSupported = function () {
    return !!navigator.mediaDevices;
};

window.startVideo = async function (videoElement) {
    //console.log("starting...")
    try {
        console.log("starting...")
        const stream = await navigator.mediaDevices.getUserMedia({ video: true });
        videoElement.srcObject = stream;
        return stream;
    } catch (error) {
        console.error('Error starting video:', error);
        throw error;
    }
};

window.stopVideo = function (videoElement) {
    const stream = videoElement.srcObject;
    const tracks = stream.getTracks();
    tracks.forEach(track => track.stop());
    videoElement.srcObject = null;
};

window.captureImage = function (videoElement, capturedImageElement) {

    try {
        const canvas = document.createElement("canvas");
        const context = canvas.getContext("2d");

        canvas.width = videoElement.clientWidth;
        canvas.height = videoElement.clientHeight;
        context.drawImage(videoElement, 0, 0, canvas.width, canvas.height);

        const imageDataUrl = canvas.toDataURL("image/png",0.9);
        capturedImageElement.src = imageDataUrl;
        console.log("Completed the update on the picture");
        console.log(imageDataUrl);
        // return imageDataUrl;
       //include code to send to the server
    }
    catch (error) {
        console.error('Error capturing image:', error);
        throw error;
    }
};

window.displayImage = function (capturedImageElement, imageDataUrl) {
    try {
        capturedImageElement.src = imageDataUrl;
        capturedImageElement.style.display = "block";
    }
    catch (error) {
        console.error('Error displaying image:', error);
        throw error;
    }
};

window.captureImage2 = function (videoElement, capturedImageElement) {
    return new Promise((resolve, reject) => {
        try {
            const canvas = document.createElement("canvas");
            const context = canvas.getContext("2d");

            canvas.width = videoElement.clientWidth;
            canvas.height = videoElement.clientHeight;
            context.drawImage(videoElement, 0, 0, canvas.width, canvas.height);

            const imageDataUrl = canvas.toDataURL("image/png");
            //capturedImageElement.src = imageDataUrl;
            console.log("Completed the update on the picture");
            console.log(imageDataUrl);

            // Resolve with the imageDataUrl
            resolve(imageDataUrl);

            // You can also include code to send data to the server here if needed
        } catch (error) {
            console.error('Error capturing image:', error);
            reject(error);
        }
    });
};

window.captureImage3 = function (videoElement) {
    return new Promise((resolve, reject) => {
        try {
            const canvas = document.createElement("canvas");
            const context = canvas.getContext("2d");
     
            canvas.width = videoElement.clientWidth;
            canvas.height = videoElement.clientHeight;
            context.drawImage(videoElement, 0, 0, canvas.width, canvas.height);
            console.log(canvas.toDataURL());

            const imageDataUrl = canvas.toDataURL();

            resolve(imageDataUrl);
            // You can also include code to send data to the server here if needed
        } catch (error) {
            console.error('Error capturing image:', error);
            reject(error);
        }
    });
};
