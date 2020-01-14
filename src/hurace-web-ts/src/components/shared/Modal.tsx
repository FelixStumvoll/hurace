import React from 'react';
import styled from 'styled-components';
import { Card } from '../../theme/CustomComponents';

const ModalHost = styled.div`
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(128, 128, 128, 0.7);
    position: absolute;
    display: flex;
`;

const ModalContent = styled(Card)`
    margin: auto;
`;

export const Modal: React.FC = ({ children }) => {
    return (
        <ModalHost>
            <ModalContent>{children}</ModalContent>
        </ModalHost>
    );
};
