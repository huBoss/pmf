function [  ] = runLoop( tag, t )
%ѭ������runAcquireAndSearchFunc.m times��
%   - tag �����ڱ������ݵ��ļ�������
%   - times ѭ������

if isempty(t) || t==0
    t=500;
end
if isempty(tag)
    tag = datestr(now,'yy-mm-dd_HH_MM_SS');
end

eloop = zeros(8,t);
mkdir(['data/' tag '/']);
for ii=1:t
    disp(['--------------' num2str(ii) '-----------------']);
    [p, i, e0, e, errmap, data] = runAcquireAndSearchFunc([],[],false); 
    plot(e,'.-');
    title('e');
    save(['data/' tag '/' num2str(ii,'%02d') datestr(now,'_yy-mm-dd_HH_MM_SS')],'p', 'i', 'e0', 'e','errmap', 'data');
    eloop(:,ii) = e';
%     pause(0.1);
    drawnow;
end
save(['data/' tag '/eloop'], 'eloop');
plot(eloop');
legend('1','2','3','4','5','6','7','8');
title('eloop');


end

