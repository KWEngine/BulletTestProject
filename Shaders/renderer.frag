#version 430

in		vec3 vPosition;
in		vec3 vNormal;

out		vec4 color;

uniform vec3 uColor;

vec3 _lightPos = vec3(100, 100, 100);
vec3 _ambient = vec3(0.25);

void main()
{
	vec3 surfaceNormal = normalize(vNormal);
	vec3 surfaceToLight = normalize(_lightPos - vPosition);
	float dotproduct = max(dot(vNormal, surfaceToLight), 0.0);

	vec3 finalColor = _ambient + uColor * dotproduct;
	color = vec4(finalColor, 1.0);
}