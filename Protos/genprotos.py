from pathlib import Path 
import os
import sys
import shutil

def gen(input_path, output_path):
    output_path = Path(output_path).resolve()
    if not os.path.exists(output_path):
        os.makedirs(output_path)

    os.chdir(input_path)
    for subdir, dirs, files in os.walk(input_path):
        for dir in dirs:
            path = Path(output_path).joinpath(subdir).joinpath(dir)
            print("mkdir", path)
            if not os.path.exists(path):
                os.mkdir(path)

    for subdir, dirs, files in os.walk("."):
        for file in files:
            if os.path.splitext(file)[1] == ".proto":
                print(f"buf generate --path {os.path.join(subdir, file)} -o {os.path.join(output_path, subdir)}")
                os.system(f"buf generate --path {os.path.join(subdir, file)} -o {os.path.join(output_path, subdir)}")


if __name__ == "__main__":
    if len(sys.argv) == 1:
        buf_registry_name = "buf.build/pujak/timej"
        output_proto_path = "."
        gen_output_path = "../obj/genprotos"
    elif len(sys.argv) != 4:
        print(
"""
Usage: genprotos.py [ buf_registry_name output_proto_path gen_output_path ]
    empty call equivalent to : genprotos.py buf.build/pujak/timej . ../obj/genprotos
""")
        exit(1)
    else:
        buf_registry_name = sys.argv[1]
        output_proto_path = sys.argv[2]
        gen_output_path = sys.argv[3]

    if os.path.exists(gen_output_path):
        shutil.rmtree(gen_output_path)

    path = f"buf export {buf_registry_name} -o {output_proto_path}"
    print(path)
    os.system(path)
    gen(output_proto_path, gen_output_path)
