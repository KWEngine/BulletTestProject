#version 430

in		vec3 aPosition;
in		vec3 aNormal;

out		vec3 vNormal;

uniform mat4 uMVP;
uniform mat4 uNormalMatrix;

void main()
{
	vNormal = (uNormalMatrix * vec4(aNormal, 0.0)).xyz;
	gl_Position = uMVP * vec4(aPosition, 1.0);
}