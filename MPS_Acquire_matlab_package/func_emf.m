function [ err, grad2 ] = func_emf( m )
%为方便使用fminunc函数而将原来的emf函数进行转换。同时为了适应minFunc都是使用列向量，这里进行了转置，如不需要则使用func-emf_raw.m。
%此文件已经编译成p文件，如要应用改变，这需要重新编译pcode
%   m - 输入矩阵（默认为8x1的矩阵），为cable的数值m = [cable.p,cable.i];
%   Author: Junyuan Hong
%   Date: 2014-10-06

    % 全局变量，需要在外部设置好
    % c 线圈参数
    % k 计算电动势用的系数
    % n ??
    % e0 测量得到的电动势
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
%         grad(:,ii) = 100*grad(:,ii)./(100*err(:,1) + 1); % 这里有问题
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

