% figure;
grid on;
% axis normal;
ipaddr='192.168.1.238';
port=8017;

sampleRate = 64000; % 64k sps
bulkSize = 1024;
bulks = 1;
bufSize = bulks * bulkSize;
TimeLen = bufSize / sampleRate;
Time = linspace(-TimeLen/2, TimeLen/2, bufSize);

channels = 3;
[data] = mps_mex86(bulks);
m = repmat( mean(data,2),1,bufSize);
data = data-m;
plot(1:(bufSize), data(1:channels,1:bufSize));

t=tcpip(ipaddr,port,'NetworkRole','client');
set(t,'OutputBufferSize',96888);
set(t,'ByteOrder','littleEndian');

while (1)
    data = mps_mex86(bulks) - m;
    
% send primitive wave data
    sendData=[Time,reshape(data',1,8*bufSize*bulks)];
    fopen(t);
    fwrite(t,sendData,'double');
    fclose(t);
    
%     plot(Time, data(1:channels,1:(bufSize)));
%     xlabel('time');
%     ylabel('voltage/V');
    
    [AAf, ff, pph, y] = mFilter(Time, data(1,:), 70, 1000);
    plot(Time, y);
    
    axis([(-TimeLen/2) (TimeLen/2) -0.1 0.1]);
    drawnow;

% send FFT data    
%    sendFFTData=[Time,y];
%    fopen(t);
%    fwrite(t,sendFFTData,'double');
%    fclost(t);
end

