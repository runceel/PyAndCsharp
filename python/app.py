import pandas as pd
import numpy as np
import time


def multiply_to_int(x, y):
    return np.where(x > 0, (x * y + 0.0000001).astype(np.int), (x * y - 0.0000001).astype(np.int))

start = time.time()

df = pd.read_csv('test.csv')
# 以下は、MessagePackとpicklの場合
# df = pd.read_msgpack('test.msgpack')
# df = pd.read_pickle('test.pkl')
df['z'] = multiply_to_int(df['x'].values, df['y'].values)
df_group = df[['a', 'z']].groupby('a').sum()
df_group['a'] = df_group.index
df_group[['a', 'z']].to_json('result.json', orient='records')

end = time.time()
print(f"所要時間:{end - start}[sec]")
