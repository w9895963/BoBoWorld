




void MY_float( float2 UVin , float Width, out float2 UVout) {

    float x = UVin.x ;

    float index = floor(x);
    float row =  floor (sqrt (index));
    float subIndex = index -  pow (row, 2);
    float2 coor;
    if (subIndex <= row) {
        coor =  float2 (subIndex, row);
    } else {
        coor = float2 (row, 2 * row - subIndex);
    }
 
 
    UVout =( coor+0.5)/(Width);
}


