
function showImg(imgs) {
    var img = imgs; //图片对象
    var imgArea = getNaturalWidthAndHeight(img);

    var layerWidth = 0;
    var layerHeight = 0;

    //alert("width:" + $(window).width() + "   height:" + $(window).height())
    //alert("imgwidth:" + imgArea[0] + "   imgheight:" + imgArea[1])

        layerHeight = $(window).height() * 0.8;
        layerWidth = imgArea[0] * (layerHeight / imgArea[1])

        if (layerWidth > $(window).width())
        {
            layerWidth = $(window).width() * 0.8;
            layerHeight = imgArea[1] * (layerWidth / imgArea[0])
        }
      
    //layer弹出层插件
    layui.use(['layer'], function () {
        var layer = layui.layer;
        layer.open({
            type: 1,
            title: false,
            closeBtn: 0,
            area: ['' + layerWidth + 'px', '' + layerHeight + 'px'],
            skin: '#FFFFFF', //没有背景色
            shadeClose: true,
            closeBtn: 1,
            content: "<center><img  width='" + layerWidth + "' height='" + layerHeight + "' src='" + imgs.src + "'></center>"
        });
    })
}
//获取图片原始宽度
function getNaturalWidthAndHeight(img) {
    var image = new Image();
    image.src = img.src;
    return [image.width, image.height];
}