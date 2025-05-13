Added below line in .csproj so that to copy template folder to build output folder

  <ItemGroup>
    <Content Include="Templates\**">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
      <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory>
    </Content>
  </ItemGroup>