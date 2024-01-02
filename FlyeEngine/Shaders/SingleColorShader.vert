#version 330 core
layout (location = 0) in vec3 inPosition;
layout (location = 1) in vec3 inNormal;

out vec3 FragPos;
out vec3 Normal;

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform mat3 modelNormalMatrix;

void main()
{
	gl_Position = vec4(inPosition, 1.0) * modelMatrix * viewMatrix * projectionMatrix;
	FragPos = vec3(vec4(inPosition, 1.0) * modelMatrix);
	Normal = inNormal * modelNormalMatrix;
}