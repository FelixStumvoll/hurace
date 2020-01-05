import React from 'react';
import styled from 'styled-components';
import { BackLinkWrapper } from '../shared/BackLinkWrapper';
import { SeasonDetailPanel } from './SeasonDetailPanel';
import { DisciplineView } from './discipline-view/DisciplineView';

const SeasonPanel = styled.div`
    display: flex;
    flex-direction: column;
    overflow: hidden;
`;

const SeasonDetailWrapper = styled.div`
    display: flex;
`;

export const SeasonDetailView: React.FC<{ seasonId: number }> = ({
    seasonId
}) => {
    return (
        <BackLinkWrapper url="/season" backText="Zurück zur Saisonübersicht">
            <SeasonPanel>
                <SeasonDetailWrapper>
                    <SeasonDetailPanel seasonId={seasonId} />
                </SeasonDetailWrapper>
                <DisciplineView seasonId={seasonId} />
            </SeasonPanel>
        </BackLinkWrapper>
    );
};
