from pathlib import Path
from argparse import ArgumentParser
import os
import shutil


def gen(input_path, output_path):
    output_path = Path(output_path).resolve()
    if not output_path.exists():
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
                print(
                    f"buf generate --path {os.path.join(subdir, file)} -o {os.path.join(output_path, subdir)}")
                os.system(
                    f"buf generate --path {os.path.join(subdir, file)} -o {os.path.join(output_path, subdir)}")


if __name__ == "__main__":
    parser = ArgumentParser(description="""    
    empty call equivalent to : genprotos.py buf.build/pujak/timej . ../obj/genprotos
                            """)
    parser.add_argument(
        'buf_registry_name', help='enter the filename of the image to process', type=str, default='buf.build/pujak/timej', nargs='?')
    parser.add_argument(
        'output_proto_path', help='path of output proto', type=str, default='.', nargs='?')
    parser.add_argument(
        'gen_output_path', help='generated path for output', type=str, default='../obj/genprotos', nargs='?')
    args = parser.parse_args()
    
    if Path(args.gen_output_path).exists():
        shutil.rmtree(gen_output_path)
    path = f"buf export {args.buf_registry_name} -o {args.output_proto_path}"
    print(path)
    os.system(path)
    gen(args.output_proto_path, args.gen_output_path)
