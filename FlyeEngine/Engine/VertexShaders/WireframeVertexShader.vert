#version 330 core
layout (location = 0) in vec3 inPosition;

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

void main()
{
	gl_Position = vec4(inPosition, 1.0) * modelMatrix * viewMatrix * projectionMatrix;
}