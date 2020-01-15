import React from 'react';
import styled from 'styled-components';
import { SkierDetailPanel } from './SkierDetailPanel';
import { DetailViewWrapper } from '../shared/DetailViewWrapper';
import { RowFlex } from '../../theme/CustomComponents';

const SkierGrid = styled(RowFlex)`
    height: fit-content;
`;

export const SkierDetailView: React.FC<{ skierId: number }> = ({ skierId }) => {
    return (
        <DetailViewWrapper
            url="/skier"
            backText="Zurück zur Fahrerübersicht"
            editConfig={{
                editText: 'Rennfahrer bearbeiten',
                editUrl: `/skier/${skierId}/update`
            }}
        >
            <SkierGrid>
                <SkierDetailPanel skierId={skierId} />
            </SkierGrid>
        </DetailViewWrapper>
    );
};
