//OL style support functions
function OLPointCircle(style) {
    return new ol.style.Circle(style);
}
function OLPointSquare(style) {
    var combined = Object.assign(style, {
        points: 4,
        angle: Math.PI / 4
    });
    return new ol.style.RegularShape(combined);
}
function OLPointTriangle(style) {
    var combined = Object.assign(style, {
        points: 3,
        angle: 0
    });
    return new ol.style.RegularShape(combined);
}
function OLPointStar(style) {
    var combined = Object.assign(style, {
        points: 5,
        radius2: 4,
        angle: 0
    });
    return new ol.style.RegularShape(combined);
}
function OLPointCross(style) {
    //TODO: Can we make the cross thicker somehow so the colors actually are visible?
    var combined = Object.assign(style, {
        points: 4,
        radius2: 0,
        angle: 0
    });
    return new ol.style.RegularShape(combined);
}
function OLPointX(style) {
    //TODO: Can we make the X thicker somehow so the colors actually are visible?
    var combined = Object.assign(style, {
        points: 4,
        radius2: 0,
        angle: Math.PI / 4
    });
    return new ol.style.RegularShape(combined);
}