#version 430

in		vec3 aPosition;
in		vec3 aNormal;

out		vec3 vPosition;
out		vec3 vNormal;

uniform mat4 uMVP;
uniform mat4 uModelMatrix;
uniform mat4 uNormalMatrix;

void main()
{
	vPosition = (uModelMatrix * vec4(aPosition, 1.0)).xyz;
	vNormal = (uNormalMatrix * vec4(aNormal, 0.0)).xyz;
	gl_Position = uMVP * vec4(aPosition, 1.0);
}