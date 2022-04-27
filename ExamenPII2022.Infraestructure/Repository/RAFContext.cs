using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExamenPII2022.Infraestructure.Repository
{
    public class RAFContext
    {
        #region INITS

        private string fileName;
        private int size;
        private const string directoryName = "DATA";
        private string DirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), directoryName);

        public RAFContext()
        {

        }

        public RAFContext(int Size, string Name)
        {
            this.size = Size;
            this.fileName = Name;
        }

        public Stream HeaderStream
        {
            get => File.Open($"{fileName}.hd", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        public Stream DataStream
        {
            get => File.Open($"{fileName}.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        //Se ocupa para crear una cabecera temporal en el método Delete
        public Stream NewHaderStream
        {
            get => File.Open($"Nueva.hd", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        #endregion

        #region CRUD

        public void Add<T>(T t)
        {
            using (BinaryWriter bwHeader = new BinaryWriter(HeaderStream),
                                   bwData = new BinaryWriter(DataStream))
            {
                int n, k;
                //Objetos de lectura de cabecera
                using (BinaryReader brHeader = new BinaryReader(bwHeader.BaseStream))
                {
                    //Si se acaba de crera el archivo entonces:
                    if (brHeader.BaseStream.Length == 0)
                    {
                        n = 0;
                        k = 0;
                    }
                    //Sino hay archivos entonces obtnemos la cantidad de objetos guardados(n), id consecutivos(k)
                    else
                    {
                        brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                        n = brHeader.ReadInt32();
                        k = brHeader.ReadInt32();
                    }
                    //calculamos la posicion en Data
                    long pos = k * size;
                    //Apuntando al registro del archivo donde se creara el nuevo registro
                    bwData.BaseStream.Seek(pos, SeekOrigin.Begin);

                    //Obteniendo la propiedades y metodos que tiene
                    PropertyInfo[] info = t.GetType().GetProperties();

                    foreach (PropertyInfo pinfo in info)
                    {
                        //Obteniendo la propiedad: su tipo, nombre y su valor 
                        Type type = pinfo.PropertyType;
                        var obj = pinfo.GetValue(t, null);

                        if (obj == null)
                        {
                            continue;
                        }
                        else
                        {
                            if (!type.IsPrimitive && type.IsClass && type != Type.GetType("System.String"))
                            {
                                PropertyInfo[] propertyInfos = obj.GetType().GetProperties();

                                foreach (var data in propertyInfos)
                                {
                                    if (data.Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        var id = data.GetValue(obj);
                                        bwData.Write((int)id);
                                    }

                                    if (data.Name.Equals("Name", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        var id = data.GetValue(obj);
                                        bwData.Write((string)id);
                                    }
                                }
                            }
                        }

                        //si es de tipo T
                        if (type.IsGenericType)
                        {
                            continue;
                        }

                        //Guardando id del registro
                        if (pinfo.Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase))
                        {
                            bwData.Write(++k);
                            continue;
                        }

                        //Buscando el tipo de la propiedad a escribir
                        if (type == typeof(int))
                        {
                            bwData.Write((int)obj);
                        }
                        else if (type == typeof(long))
                        {
                            bwData.Write((long)obj);
                        }
                        else if (type == typeof(float))
                        {
                            bwData.Write((float)obj);
                        }
                        else if (type == typeof(double))
                        {
                            bwData.Write((double)obj);
                        }
                        else if (type == typeof(decimal))
                        {
                            bwData.Write((decimal)obj);
                        }
                        else if (type == typeof(char))
                        {
                            bwData.Write((char)obj);
                        }
                        else if (type == typeof(bool))
                        {
                            bwData.Write((bool)obj);
                        }
                        else if (type == typeof(string))
                        {
                            bwData.Write((string)obj);
                        }
                        else if (type.IsEnum)
                        {
                            bwData.Write((int)obj);
                        }
                    }
                    //Escribiendo el id nuevo en la pos correspondiente
                    long posh = 8 + n * 4;
                    bwHeader.BaseStream.Seek(posh, SeekOrigin.Begin);
                    bwHeader.Write(k);

                    bwHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                    bwHeader.Write(++n);
                    bwHeader.Write(k);
                }
            }
        }

        public bool Delete<T>(int t)
        {
            using (BinaryWriter bwHeader = new BinaryWriter(HeaderStream),
                   /*Creando nueva cabecera*/ bwNewHeader = new BinaryWriter(NewHaderStream))
            {
                int n, k, idtemp, pos;
                using (BinaryReader brHeader = new BinaryReader(bwHeader.BaseStream))
                {
                    brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                    n = brHeader.ReadInt32();
                    k = brHeader.ReadInt32();

                    //id fuera de rango [1 , k]
                    if (t <= 0 || t > k)
                    {
                        return false;
                    }

                    //Validando que el id exista en la cabecera y encontrando ese id
                    if (!binary_search(n, t, brHeader, out int index))
                    {
                        return false;
                    }

                    //Copiando cabecera 
                    bwNewHeader.Seek(0, SeekOrigin.Begin);
                    bwNewHeader.Write(n - 1);
                    bwNewHeader.Write(k);

                    for (int i = 0; i < n; i++)
                    {
                        pos = 8 + (i * 4);
                        brHeader.BaseStream.Seek(pos, SeekOrigin.Begin);
                        idtemp = brHeader.ReadInt32();

                        //Excepto el id a eliminar
                        if (t != idtemp)
                        {
                            bwNewHeader.Write(idtemp);
                        }
                    }
                }
            }

            //Eliminando cabecera vieja 
            File.Delete($"{fileName}.hd");
            //Renombrando cabecera nueva
            File.Copy("Nueva.hd", $"{fileName}.hd");
            File.Delete("Nueva.hd");

            return true;
        }

        //Al utilizar el metodo se entiende que el element(id) puede estar dentro el rango [1, k]
        private bool binary_search(int n, int element, BinaryReader brHeader, out int idbuscado)
        {
            int pos = 0;
            int posh;
            int max = n - 1;
            int min = 0;

            //Dandole a variables de salida valores por defecto por si no encuentran el id en la cabecera
            idbuscado = -1;
            bool encontrado = false;

            while (min <= max)
            {
                //Obteniendo la posicion del centro 
                pos = (max + min) / 2;
                posh = 8 + (pos * 4);
                brHeader.BaseStream.Seek(posh, SeekOrigin.Begin);

                //Elemento de la posición
                int temp = brHeader.ReadInt32();

                //Verificando si el elemento leido es el que queriamos buscar
                if (temp == element)
                {
                    idbuscado = element;
                    encontrado = true;
                    break;
                }
                else if (temp > element)
                {
                    max = pos - 1;
                }
                else
                {
                    min = pos + 1;
                }
            }
            return encontrado;
        }

        public IReadOnlyList<T> GetAll<T>()
        {
            List<T> listT = new List<T>();
            int n = 0, k = 0;

            try
            {
                using (BinaryReader brHeader = new BinaryReader(HeaderStream))
                {
                    if (brHeader.BaseStream.Length > 0)
                    {
                        brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                        n = brHeader.ReadInt32();
                        k = brHeader.ReadInt32();
                    }
                }

                if (n == 0)
                {
                    return listT;
                }

                for (int i = 0; i < n; i++)
                {
                    int index;
                    using (BinaryReader brHeader = new BinaryReader(HeaderStream))
                    {
                        long posh = 8 + i * 4;
                        brHeader.BaseStream.Seek(posh, SeekOrigin.Begin);
                        index = brHeader.ReadInt32();
                    }

                    T t = Get<T>(index);

                    listT.Add(t);
                }

                return listT;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T Get<T>(int id)
        {
            //Sus propiedades tendran valor por defecto
            T newValue = (T)Activator.CreateInstance(typeof(T));

            using (BinaryReader brHeader = new BinaryReader(HeaderStream),
                                brData = new BinaryReader(DataStream))
            {
                brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                int n = brHeader.ReadInt32();
                int k = brHeader.ReadInt32();

                //Verificando que el id este entre el rango valido segun n y K
                if (id <= 0 || id > k)
                {
                    return default(T);
                }

                //Obteniendo las propiedades
                PropertyInfo[] properties = newValue.GetType().GetProperties();


                //Validando que se encuentra el objeto T con id y obteniendo el id buscado en la cabecera
                if (!binary_search(n, id, brHeader, out int index))
                {
                    //No hay necesidad de validar index porque cuando el index no es encontrado cae en este if
                    return default(T);
                }

                long posd = (index - 1) * size;

                brData.BaseStream.Seek(posd, SeekOrigin.Begin);

                //Leyendo las propiedades del objeto segun su tippo
                foreach (PropertyInfo pinfo in properties)
                {
                    Type type = pinfo.PropertyType;

                    if (newValue == null)
                    {
                        continue;
                    }
                    else
                    {
                        if (!type.IsPrimitive && type.IsClass && type != Type.GetType("System.String"))
                        {
                            PropertyInfo[] propertyInfos = type.GetProperties();
                            object objectClass = Activator.CreateInstance(pinfo.PropertyType);
                            foreach (var data in propertyInfos)
                            {
                                if (data.Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    data.SetValue(objectClass, brData.GetValue<int>(TypeCode.Int32));
                                    pinfo.SetValue(newValue, objectClass);
                                }

                                if (data.Name.Equals("Name", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    data.SetValue(objectClass, brData.GetValue<string>(TypeCode.String));
                                    pinfo.SetValue(newValue, objectClass);
                                }
                            }
                        }
                    }
                    if (type.IsGenericType)
                    {
                        continue;
                    }

                    if (type == typeof(int))
                    {
                        pinfo.SetValue(newValue, brData.GetValue<int>(TypeCode.Int32));
                    }
                    else if (type == typeof(long))
                    {
                        pinfo.SetValue(newValue, brData.GetValue<long>(TypeCode.Int64));
                    }
                    else if (type == typeof(float))
                    {
                        pinfo.SetValue(newValue, brData.GetValue<float>(TypeCode.Single));
                    }
                    else if (type == typeof(double))
                    {
                        pinfo.SetValue(newValue, brData.GetValue<double>(TypeCode.Double));
                    }
                    else if (type == typeof(decimal))
                    {
                        pinfo.SetValue(newValue, brData.GetValue<decimal>(TypeCode.Decimal));
                    }
                    else if (type == typeof(char))
                    {
                        pinfo.SetValue(newValue, brData.GetValue<char>(TypeCode.Char));
                    }
                    else if (type == typeof(bool))
                    {
                        pinfo.SetValue(newValue, brData.GetValue<bool>(TypeCode.Boolean));
                    }
                    else if (type == typeof(string))
                    {
                        pinfo.SetValue(newValue, brData.GetValue<string>(TypeCode.String));
                    }
                    else if (type.IsEnum)
                    {
                        pinfo.SetValue(newValue, brData.GetValue<int>(TypeCode.Int32));
                    }
                }

                return newValue;
            }

        }

        public bool Update<T>(T t, int id)
        {
            using (BinaryWriter bwHeader = new BinaryWriter(HeaderStream),
                                  bwData = new BinaryWriter(DataStream))
            {
                int n, k;

                using (BinaryReader brHeader = new BinaryReader(bwHeader.BaseStream))
                {
                    //Leyendo la cantidad de datos existentes y los id consecutivos
                    brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                    n = brHeader.ReadInt32();
                    k = brHeader.ReadInt32();

                    //id fuera de rango [1 , k]
                    if (id <= 0 || id > k)
                    {
                        return false;
                    }

                    //Validando que el id exista en la cabecera y encontrando ese id
                    if (!binary_search(n, id, brHeader, out int index))
                    {
                        return false;
                    }

                    //calculamos la posicion en Data
                    long pos = (index - 1) * size;

                    //Apuntando al registro del archivo donde se editara el registro
                    bwData.BaseStream.Seek(pos, SeekOrigin.Begin);

                    //Obteniendo la propiedades y metodos que tiene
                    PropertyInfo[] info = t.GetType().GetProperties();

                    foreach (PropertyInfo pinfo in info)
                    {
                        //Obteniendo la propiedad: su tipo, nombre y su valor 
                        Type type = pinfo.PropertyType;
                        object obj = pinfo.GetValue(t, null);

                        //si es de tipo T
                        if (type.IsGenericType)
                        {
                            continue;
                        }

                        //Guardando id del registro
                        if (pinfo.Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase))
                        {
                            bwData.Write(id);
                            continue;
                        }

                        //Buscando el tipo de la propiedad a escribir
                        if (type == typeof(int))
                        {
                            bwData.Write((int)obj);
                        }
                        else if (type == typeof(long))
                        {
                            bwData.Write((long)obj);
                        }
                        else if (type == typeof(float))
                        {
                            bwData.Write((float)obj);
                        }
                        else if (type == typeof(double))
                        {
                            bwData.Write((double)obj);
                        }
                        else if (type == typeof(decimal))
                        {
                            bwData.Write((decimal)obj);
                        }
                        else if (type == typeof(char))
                        {
                            bwData.Write((char)obj);
                        }
                        else if (type == typeof(bool))
                        {
                            bwData.Write((bool)obj);
                        }
                        else if (type == typeof(string))
                        {
                            bwData.Write((string)obj);
                        }
                        else if (type.IsEnum)
                        {
                            bwData.Write((int)obj);
                        }
                    }
                    return true;
                }
            }
        }

        #endregion
    }
}
