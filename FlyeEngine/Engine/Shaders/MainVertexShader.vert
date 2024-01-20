#version 330 core
layout (location = 0) in vec3 inPosition;
layout (location = 1) in vec3 inNormal;
layout (location = 2) in vec3 inTexCoord;
layout (location = 3) in vec3 inColor;

out vec3 FragPos;
out vec3 Normal;
out vec3 texCoord;
out vec3 vertColor;

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform mat3 modelNormalMatrix;

void main()
{
	gl_Position = vec4(inPosition, 1.0) * modelMatrix * viewMatrix * projectionMatrix;
	FragPos = vec3(vec4(inPosition, 1.0) * modelMatrix);
	Normal = inNormal * modelNormalMatrix;
	texCoord = inTexCoord;
	vertColor = inColor;
}