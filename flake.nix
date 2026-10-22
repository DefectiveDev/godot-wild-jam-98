{
    description = "C# dev shell";

    inputs = {
        nixpkgs.url = "github:nixos/nixpkgs/nixos-unstable";
    };

    outputs = { self, nixpkgs }:
    let
        system = "x86_64-linux";
        pkgs = import nixpkgs {inherit system;};
    in
    {

        devShell.${system} = pkgs.mkShell {
            name = "CSharp";
            packages = with pkgs; [
                # Lsp server for c# supported by neovim
                roslyn-ls
                # dot net version 2dog uses at the time of making this.
                dotnet-sdk_10
                # godot-mono uses dotnet-sdk_8 by default we need to override this to match 2dog for in editor building
                (godot-mono.overrideAttrs {
                 dotnet-sdk = pkgs.dotnet-sdk_10;
                 })
                # some builds require python so we add this as well.
                python3
                # csharp formatter
                csharpier
            ];
        };

    };
}
