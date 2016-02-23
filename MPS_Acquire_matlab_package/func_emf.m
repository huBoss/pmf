function [ err, grad2 ] = func_emf( m )
%Ϊ����ʹ��fminunc��������ԭ����emf��������ת����ͬʱΪ����ӦminFunc����ʹ�������������������ת�ã��粻��Ҫ��ʹ��func-emf_raw.m��
%���ļ��Ѿ������p�ļ�����ҪӦ�øı䣬����Ҫ���±���pcode
%   m - �������Ĭ��Ϊ8x1�ľ��󣩣�Ϊcable����ֵm = [cable.p,cable.i];
%   Author: Junyuan Hong
%   Date: 2014-10-06

    % ȫ�ֱ�������Ҫ���ⲿ���ú�
    % c ��Ȧ����
    % k ����綯���õ�ϵ��
    % n ??
    % e0 �����õ��ĵ綯��
    global c k n e0 optimParam
    m = m';
    
    cb.p = m(1:3);
    cb.i = m(4:6);
    [e1,grad] = emf(cb, c, k, n);
    e1 = e1';
%     grad(isnan(grad)) = 0;
    
    for ii = 1:size(e0,2)
        err(:,ii) = e1(:) - e0(:,ii);
    end
    
%     err = e1 - e0;
    
%     for ii = 1:size(grad,2)
%         grad(:,ii) = 100*grad(:,ii)./(100*err(:,1) + 1); % ����������
%     end
%     err = log(100*err + 1);
    
%     err = err./e1;
%     err = sqrt(err'*err);
    errv = 0;
    for ii = 1:size(err,2)
        errv = errv + sqrt(err(:,ii)'*err(:,ii));
    end
    grad = 100*grad/(100*errv + 1);
    grad2 = err' * grad;
    grad2 = sum(grad2/errv,1);
    err = errv;
%     errv
%     err = exp(-10*err); % for error map
%     err
%     err = log(100*err + 1);
    
    grad2(optimParam) = 0;
    grad2 = grad2';
    
    if ~isLegal(grad2)
%         keyboard
    
        if length(grad2(isnan(grad2)))~=0
            fprintf('nan error');
            grad2(isnan(grad2)) = 0;
        end
        if isnan(err)
            err = 0;
        end
        
    end

end

