#version 330 core
layout (location = 0) in vec3 inPosition;
layout (location = 1) in vec3 inNormal;
layout (location = 2) in vec3 inTexCoord;
layout (location = 3) in vec3 inAmbient;
layout (location = 4) in vec3 inDiffuse;
layout (location = 5) in vec3 inSpecular;
layout (location = 6) in float inSpecularExponent;
layout (location = 7) in float inIllumModel;

out vec3 FragPos;
out vec3 Normal;
out vec3 texCoord;
out vec3 ambient;
out vec3 diffuse;
out vec3 specular;
out float specularExp;
out float illumModel;

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

	ambient = inAmbient;
	diffuse = inDiffuse;
	specular = inSpecular;
	specularExp = inSpecularExponent;
	illumModel = inIllumModel;
}