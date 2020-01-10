using System;

namespace Management {
    /// <summary>
    /// Classes that implement this interface should have an serialized instance of DataSettings to register through.
    /// </summary>
    public interface IDataPersister {
        DataSettings get_data_settings ( );

        void set_data_settings ( string data_tag, DataSettings._Persistence_Type persistence_type );

        Data save_data ( );

        void load_data ( Data data );
    }

    [Serializable]
    public class DataSettings {
        public enum _Persistence_Type {
            Do_Not_Persist,
            Read_Only,
            Write_Only,
            Read_Write,
        }

        public string data_tag = System.Guid.NewGuid ( ).ToString ( );
        public _Persistence_Type persistence_type = _Persistence_Type.Read_Write;

        public string to_string ( ) {
            return data_tag + " " + persistence_type.ToString ( );
        }
    }

    public class Data {

    }


    public class Data<T> : Data {
        public T value;

        public Data ( T value ) {
            this.value = value;
        }
    }


    public class Data<T0, T1> : Data {
        public T0 value0;
        public T1 value1;

        public Data ( T0 value0, T1 value1 ) {
            this.value0 = value0;
            this.value1 = value1;
        }
    }


    public class Data<T0, T1, T2> : Data {
        public T0 value0;
        public T1 value1;
        public T2 value2;

        public Data ( T0 value0, T1 value1, T2 value2 ) {
            this.value0 = value0;
            this.value1 = value1;
            this.value2 = value2;
        }
    }


    public class Data<T0, T1, T2, T3> : Data {
        public T0 value0;
        public T1 value1;
        public T2 value2;
        public T3 value3;

        public Data ( T0 value0, T1 value1, T2 value2, T3 value3 ) {
            this.value0 = value0;
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
        }
    }


    public class Data<T0, T1, T2, T3, T4> : Data {
        public T0 value0;
        public T1 value1;
        public T2 value2;
        public T3 value3;
        public T4 value4;

        public Data ( T0 value0, T1 value1, T2 value2, T3 value3, T4 value4 ) {
            this.value0 = value0;
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
            this.value4 = value4;
        }
    }
}