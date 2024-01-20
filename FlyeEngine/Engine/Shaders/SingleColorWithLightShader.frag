#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec3 vertColor;

uniform vec3 lightPosition;
uniform vec3 lightColor;
uniform vec3 viewPosition;

void main()
{
    // Ambient
    float ambientStrength = 0.1;
    vec3 ambientLight = ambientStrength * lightColor;

    // Diffuse
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPosition - FragPos);
    float diffuse = max(dot(norm, lightDir), 0.0);

    // Specular
    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPosition - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * spec * lightColor;

    //vec3 finalColor = vertColor * lightColor * diffuse;
    vec3 finalColor = (ambientLight + diffuse + specular) * vertColor;
    FragColor = vec4(finalColor, 1.0);
}