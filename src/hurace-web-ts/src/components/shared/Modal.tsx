import React from 'react';
import styled from 'styled-components';
import { Card } from '../../theme/CustomComponents';

const ModalHost = styled.div`
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(128, 128, 128, 0.7);
    overflow: hidden;
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
